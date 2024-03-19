using System.Diagnostics.CodeAnalysis;
using Insight.BuildingBlocks.Exceptions;
using Insight.BuildingBlocks.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Insight.BuildingBlocks.Integration.Inbox
{
    public class InboxPublisher
    {
        private readonly IInbox _inbox;
        private readonly ISender _sender;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<InboxPublisher> _logger;

        public InboxPublisher(IInbox inbox, ISender sender, IUnitOfWork unitOfWork, ILogger<InboxPublisher> logger)
        {
            _inbox = inbox;
            _sender = sender;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        internal async Task PublishPendingAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && await PublishBatchAsync(cancellationToken))
            { }
        }

        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>")]
        private async Task<bool> PublishBatchAsync(CancellationToken cancellationToken)
        {
            var serializer = new InboxMessageSerializer();
            await _inbox.CleanUpOldMessagesAsync();
            var messages = _inbox.GetUnProcessedMessages();
            foreach (var message in messages)
            {   
                try
                {
                    var cmd = serializer.Deserialize(message.Payload, message.MessageType);
                    if (cmd is not null)
                    {
                        try
                        {
                            await _sender.Send(cmd, cancellationToken);
                            message.Processed();
                            _inbox.Processed(message);
                        }
                        catch (NotFoundException) when (message.RetryCount < 2)
                        {
                            // retry later.
                            message.IncreaseRetryCount();
                        }
                        catch (Exception ex)
                        {
                            // We should mark the command as failed. And maybe try again?
                            _logger.LogWarning(ex, "Processing InboxMessage {Id} failed", message.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Ending here means that we cant deserialize. We should mark it as failed.
                    _logger.LogError(ex, "Processing InboxMessage {Id} failed", message.Id);
                }
            }            
            await _unitOfWork.Commit(cancellationToken);
            return false;
        }
    }
}
