using Insight.BuildingBlocks.Configuration;
using Insight.BuildingBlocks.Integration;
using Insight.BuildingBlocks.Tests.Infrastructure;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Insight.BuildingBlocks.Tests.Infrastructure
{
    public class PublishingIntegrationEventTests
    {
        internal sealed class MyEvent : IntegrationEvent
        {
            public MyEvent(Guid id, DateTimeOffset occuredOn) : base(id, occuredOn)
            {
            }
        }

        internal sealed class MyEventIntegrationEventHandler : IIntegrationEventListener<MyEvent>
        {
            public Task Handle(MyEvent notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        internal sealed class MyEventIntegrationEventHandler2 : IIntegrationEventListener<MyEvent>
        {
            public Task Handle(MyEvent notification, CancellationToken cancellationToken)
            {
                return Task.CompletedTask;
            }
        }

        // [Fact]
        // public Task Will_Publish_IntegrationEvent_To_Handler()
        // {
            // var services = new ServiceCollection();
            // services.UseBuildingBlocks();
            //
            // services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Insight.,Certificates.Application.AssemblyReference.Assembly));
            // services.AddMediatR(typeof(PublishingIntegrationEventTests), typeof(Test2.MyEventIntegrationEventHandler));
            //
            // var serviceProvider = services.BuildServiceProvider();
            //
            // var handlers = serviceProvider
            //     .GetServices<INotificationHandler<MyEvent>>()
            //     .ToList();
            // Assert.Equal(3, handlers.Count);
            //
            // var mediator = serviceProvider.GetRequiredService<IMediator>();
            //
            // var @event = new MyEvent(Guid.NewGuid(), DateTimeOffset.UtcNow);
            // await mediator.Publish(@event);
            //
            // var publisher = serviceProvider.GetRequiredService<IIntegrationEventPublisher>();
            //
            // await publisher.Publish(@event, CancellationToken.None);
            //
            // Assert.All(handlers,
            //     t => Assert.True(t is IIntegrationEventListener<MyEvent>)
            //     );
            // Assert.True(false, "How to add mediatr");
        // }
    }
}

namespace Test2
{
    internal sealed class MyEventIntegrationEventHandler : IIntegrationEventListener<PublishingIntegrationEventTests.MyEvent>
    {
        public Task Handle(PublishingIntegrationEventTests.MyEvent notification, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
