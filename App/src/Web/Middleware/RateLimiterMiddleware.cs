using System.Threading.RateLimiting;

namespace Web.Middleware
{
    public static class RateLimiterMiddleware
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddRateLimiter(options =>
            {
                options.AddPolicy("ipPolicy", httpContext =>
                {
                    string ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    string path = httpContext.Request.Path.ToString();
                    return RateLimitPartition.GetFixedWindowLimiter(
                        partitionKey: $"{ip}:{path}",
                        factory: _ => new FixedWindowRateLimiterOptions
                        {
                            PermitLimit = 10,
                            Window = TimeSpan.FromSeconds(1),
                            QueueLimit = 5,
                        }
                    );
                });
            });
        }
    }
}
