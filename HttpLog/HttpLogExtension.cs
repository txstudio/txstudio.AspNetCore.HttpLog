using Microsoft.AspNetCore.Builder;

namespace HttpLogExtensions
{
    public static class HttpLogExtension
    {
        public static IApplicationBuilder UseHttpLog(this IApplicationBuilder builer)
        {
            return builer.UseMiddleware<HttpLogMiddleware>(null);
        }

        public static IApplicationBuilder UseHttpLog(this IApplicationBuilder builer, IHttpLogStore httpLogStore)
        {
            return builer.UseMiddleware<HttpLogMiddleware>(httpLogStore);
        }
    }
}
