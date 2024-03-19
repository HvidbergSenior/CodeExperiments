using System.Diagnostics;

namespace Insight.WebApplication.Configuration
{
    public static class AppBuilderExtensions
    {
        private static string TraceIdKey = "X-Trace-Id";
        public static IApplicationBuilder UseTraceIds(this IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                if (context.Request.Headers.TryGetValue(TraceIdKey, out var value))
                {
                    Activity.Current?.SetParentId(value!);
                }
                context.Response.Headers.Add(TraceIdKey, Activity.Current?.Id ?? context?.TraceIdentifier);
                await next.Invoke();
            });

            return app;
        }
    }
}
