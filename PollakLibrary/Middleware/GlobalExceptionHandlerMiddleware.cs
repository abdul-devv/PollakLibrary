using System.Net;
using System.Text.Json;
namespace PollakLibrary.Api.Middleware;
public class GlobalExceptionHandlerMiddleware {
    private readonly RequestDelegate _next;
    public GlobalExceptionHandlerMiddleware(RequestDelegate next) => _next = next;
    public async Task InvokeAsync(HttpContext context) {
        try { await _next(context); }
        catch (Exception ex) {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsync(JsonSerializer.Serialize(new { error = ex.Message }));
        }
    }
}
