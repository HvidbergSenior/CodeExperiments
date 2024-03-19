using System.Diagnostics.CodeAnalysis;
using Insight.BuildingBlocks.Infrastructure;
using Microsoft.Extensions.Logging;

namespace Insight.BuildingBlocks.Integration.Outbox
{
    public sealed class OutboxPublisher
    {
        private readonly IOutbox _outbox;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IIntegrationEventPublisher _integrationEventPublisher;
        private readonly ILogger<OutboxPublisher> _logger;

        public OutboxPublisher(IOutbox outbox, IUnitOfWork unitOfWork,
            IIntegrationEventPublisher integrationEventPublisher, ILogger<OutboxPublisher> logger)
        {
            _outbox = outbox;
            _unitOfWork = unitOfWork;
            _integrationEventPublisher = integrationEventPublisher;
            _logger = logger;
        }

        [SuppressMessage("Performance", "CA1848:Use the LoggerMessage delegates", Justification = "<Pending>")]
        [SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "<Pending>")]
        internal async Task<bool> PublishPendingAsync(CancellationToken cancellationToken)
        {
            var serializer = new OutboxMessageSerializer();
            await _outbox.CleanUpOldMessagesAsync();
            var messages = _outbox.GetUnProcessedMessages().ToList();
            foreach (var message in messages)
            {
                try
                {
                    var cmd = serializer.Deserialize(message.Payload, message.MessageType);
                    if (cmd is not null)
                    {
                        try
                        {
                            await _integrationEventPublisher.Publish((IIntegrationEvent)cmd, cancellationToken);
                            message.Processed();
                        }
                        catch (Exception ex)
                        {
                            // We should mark the command as failed. And maybe try again?
                            _logger.LogError(ex, "Processing OutboxMessage {Id} failed", message.Id);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Deserializing OutboxMessage {Id} failed", message.Id);
                }
            }

            try
            {
                _outbox.Processed(messages);
                await _unitOfWork.Commit(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Committing Outbox failed");
                throw;
            }

            return false;
        }
    }
}