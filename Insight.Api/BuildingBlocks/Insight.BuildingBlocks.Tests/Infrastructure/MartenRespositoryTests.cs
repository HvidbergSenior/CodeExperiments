using Insight.BuildingBlocks.Infrastructure.Marten;
using Insight.BuildingBlocks.Fakes;
using Marten;
using Marten.Services;
using Newtonsoft.Json;
using Testcontainers.PostgreSql;
using Weasel.Core;

namespace Insight.BuildingBlocks.Tests.Infrastructure
{
    public class MartenRepositoryTests : IAsyncLifetime
    {
        private readonly PostgreSqlContainer postgreSqlContainer = new PostgreSqlBuilder()
            .WithDatabase("db")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

        [Fact]
        public async Task Can_Save_AggregatesAsync()
        {
            var eventPublisher = new FakeEntityEventsPublisher();
            var options = new StoreOptions();
            var serializer = new JsonNetSerializer
            {
                NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters,
                EnumStorage = EnumStorage.AsString,
            };

            serializer.Customize(_ =>
            {
                _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                _.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            });
            options.Serializer(serializer);
            options.Connection(postgreSqlContainer.GetConnectionString());
            var store = new DocumentStore(options);

            using (var session = store.LightweightSession())
            {
                var repository = new MartenDocumentRepository<Car>(session, eventPublisher);
                var car = Car.Create();
                await repository.Add(car);
                await repository.SaveChanges();
                await session.SaveChangesAsync();
            }
            store.Dispose();
            Assert.True(eventPublisher.HasBeenCalled);
        }

        [Fact]
        public async Task Can_Load_AggregatesAsync()
        {
            var car = Car.Create();
            var options = new StoreOptions();
            var serializer = new JsonNetSerializer
            {
                NonPublicMembersStorage = NonPublicMembersStorage.NonPublicSetters,
                EnumStorage = EnumStorage.AsString,
            };

            serializer.Customize(_ =>
            {
                _.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
                _.DateFormatHandling = DateFormatHandling.IsoDateFormat;
            });
            options.Serializer(serializer);
            options.Connection(postgreSqlContainer.GetConnectionString());
            var store = new DocumentStore(options);

            using (var writeSession = store.LightweightSession())
            {
                var repository = new MartenDocumentRepository<Car>(writeSession, new FakeEntityEventsPublisher());
                await repository.Add(car);
                await writeSession.SaveChangesAsync();
            }
            Car carFromDatabase;
            using (var readSession = store.LightweightSession())
            {
                var readRepository = new MartenDocumentRepository<Car>(readSession, new FakeEntityEventsPublisher());
                carFromDatabase = await readRepository.GetById(car.Id);
                await readSession.DisposeAsync();
            }
            store.Dispose();

            Assert.NotNull(carFromDatabase);
            Assert.Equal(car.Id, carFromDatabase.Id);
            Assert.Equal(car.Owner.Id, carFromDatabase.Owner.Id);
        }

        public Task DisposeAsync()
        {
            return postgreSqlContainer.DisposeAsync().AsTask();
        }

        public Task InitializeAsync()
        {
            return postgreSqlContainer.StartAsync();
        }
    }
}
