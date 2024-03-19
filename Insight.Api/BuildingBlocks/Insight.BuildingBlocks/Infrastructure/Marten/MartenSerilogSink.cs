using Marten.Services;
using Marten;
using Microsoft.ApplicationInsights;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;
using Serilog.Context;
using System.Diagnostics;

namespace Insight.BuildingBlocks.Infrastructure.Marten
{
    public class MartenSerilogSink : IMartenLogger, IMartenSessionLogger
    {
        private readonly ILogger logger;
        private readonly IServiceProvider serviceProvider;
        private DateTimeOffset startTime;
        private Stopwatch timer = new Stopwatch();

        public MartenSerilogSink(IServiceProvider serviceProvider)
        {
            this.logger = serviceProvider.GetRequiredService<ILogger<MartenSerilogSink>>();
            this.serviceProvider = serviceProvider;
        }

        public void LogFailure(NpgsqlCommand command, Exception ex)
        {
            using (LogContext.PushProperty("Marten Exception", ex))
            {
                if (command == null)
                {
                    logger.LogError("Marten: Postgresql command is nonexistent");
                    return;
                }
                using (LogContext.PushProperty("Marten Command", command.CommandText))
                {
                    logger.LogWarning("Marten: Postgresql command failed!");
                    timer.Stop();
                    var msg = $"Marten Command {command.CommandText}";
                    var telemetryClient = serviceProvider.GetService<TelemetryClient>();
                    if(telemetryClient != null)
                    {
                        telemetryClient.TrackDependency("SQL", "Postgres", msg, startTime, timer.Elapsed, false);
                    }
                }
            }
        }

        public void LogSuccess(NpgsqlCommand command)
        {
            if (command == null)
            {
                logger.LogError("Marten: Postgresql command is nonexistent");
                return;
            }
            using (LogContext.PushProperty("Marten Command", command.CommandText))
            {
                logger.LogTrace("Marten: Successfully executed command");
                timer.Stop();
                var msg = $"Marten Command {command.CommandText}";
                var telemetryClient = serviceProvider.GetService<TelemetryClient>();
                if(telemetryClient != null)
                {
                    telemetryClient.TrackDependency("SQL", "Postgres", msg, startTime, timer.Elapsed, true);
                }
            }
        }

        public void OnBeforeExecute(NpgsqlCommand command)
        {
            if (command == null)
            {
                logger.LogError("Marten: Postgresql command is nonexistent");
                return;
            }
        }

        public void RecordSavedChanges(IDocumentSession session, IChangeSet commit)
        {
            if (commit == null)
            {
                logger.LogWarning("Marten: Recoding saved changes was called with an empty commit change set");
                return;
            }
            timer.Stop();
            var msg = $"Marten: Persisted {commit.Updated.Count()} updates, {commit.Inserted.Count()} inserts, and {commit.Deleted.Count()} deletions";
            var telemetryClient = serviceProvider.GetService<TelemetryClient>();
            if(telemetryClient != null)
            {
                telemetryClient.TrackDependency("SQL", "Postgres", msg, startTime, timer.Elapsed, true);
            }
            logger.LogDebug(msg);
        }

        public void SchemaChange(string sql)
        {
            using (LogContext.PushProperty("Marten Command", sql))
            {
                logger.LogInformation("Marten: Executing DDL change");
            }
        }

        public IMartenSessionLogger StartSession(IQuerySession session)
        {
            startTime = DateTimeOffset.UtcNow;
            timer = Stopwatch.StartNew();
            return this;
        }
    }
}
