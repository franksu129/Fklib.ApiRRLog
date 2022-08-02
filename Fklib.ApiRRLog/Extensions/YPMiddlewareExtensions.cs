using Microsoft.AspNetCore.Builder;
using Fklib.ApiRRLog.Middleware;

namespace Fklib.ApiRRLog.Extensions
{
    public static class ApiLogMiddlewareExtensions
    {
        public static IApplicationBuilder UseApiLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiLoggingMiddleware>();
        }
    }
}