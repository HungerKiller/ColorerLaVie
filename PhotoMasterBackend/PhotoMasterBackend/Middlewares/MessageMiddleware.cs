using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace PhotoMasterBackend.Middlewares
{
    public class MessageMiddleware
    {
        private readonly RequestDelegate _next;

        public MessageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Call the next delegate/middleware in the pipeline
            await _next(context);

            if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
            {
                await context.Response.WriteAsync($"Unauthorized. Need login.");
            }
            else if (context.Response.StatusCode == StatusCodes.Status403Forbidden)
            {
                await context.Response.WriteAsync($"Forbidden. Need login as other authorized user (admin).");
            }
        }
    }
}
