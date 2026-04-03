using System.Net;
using System.Text.Json;
using DevJobsAPI.Models.Error;


namespace DevJobsAPI.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context); // Try to run the next piece of code
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred."); // Log it for YOU to see
                await HandleExceptionAsync(context, ex); // Send a polite message to the USER
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new ErrorResponse
            {
                StatusCode = context.Response.StatusCode,
                ErrorMessage = "Internal Server Error. Our team has been notified. Please try again later."
            };

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}