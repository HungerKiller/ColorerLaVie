using Microsoft.AspNetCore.Builder;

namespace PhotoMasterBackend.Middlewares
{
    public static class MessageMiddlewareExtensions
    {
        public static IApplicationBuilder UseMessage(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<MessageMiddleware>();
        }
    }
}
