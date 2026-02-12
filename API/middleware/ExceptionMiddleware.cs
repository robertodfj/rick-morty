using RickYMorty.middleware;

namespace RickYMorty.middleware
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
                await _next(context);
            }
            catch (ApiException apiEx)
            {
                _logger.LogError(apiEx, "API Exception occurred");
                context.Response.StatusCode = apiEx.StatusCode;
                context.Response.ContentType = "application/json";
                var response = new { message = apiEx.Message };
                await context.Response.WriteAsJsonAsync(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/json";
                var response = new { message = "An unexpected error occurred." };
                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}