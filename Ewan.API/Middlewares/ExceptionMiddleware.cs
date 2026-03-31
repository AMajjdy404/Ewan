using Ewan.API.Errors;

namespace Ewan.API.Middlewares
{
    using Microsoft.IdentityModel.Tokens;
    using Serilog.Context;
    using System.Diagnostics;
    using System.Text.Json;

    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            IWebHostEnvironment env,
            ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                stopwatch.Stop();

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning("Cannot handle exception because the response has already started.");
                    LogError(context, ex, stopwatch.Elapsed.TotalMilliseconds);
                    throw;
                }

                await HandleExceptionAsync(context, ex, stopwatch.Elapsed.TotalMilliseconds);
            }
            finally
            {
                stopwatch.Stop();
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex, double elapsedMilliseconds)
        {
            var (statusCode, message) = ex switch
            {
                UnauthorizedAccessException => (
                    StatusCodes.Status401Unauthorized,
                    "Unauthorized access."
                ),

                SecurityTokenException => (
                    StatusCodes.Status401Unauthorized,
                    "Invalid or expired token."
                ),

                BadHttpRequestException => (
                    StatusCodes.Status400BadRequest,
                    ex.Message
                ),

                InvalidOperationException => (
                    StatusCodes.Status400BadRequest,
                    ex.Message
                ),

                KeyNotFoundException => (
                    StatusCodes.Status404NotFound,
                    ex.Message
                ),

                _ => (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred. Please try again later."
                )
            };

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            ApiErrorResponse response;

            if (_env.IsDevelopment() && statusCode == StatusCodes.Status500InternalServerError)
            {
                response = new ApiErrorResponse(statusCode, message, ex.Message);
            }
            else
            {
                response = new ApiErrorResponse(statusCode, message);
            }

            await WriteJsonResponse(context, response);

            LogError(context, ex, elapsedMilliseconds);
        }

        private async Task WriteJsonResponse(HttpContext context, ApiErrorResponse response)
        {
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, options));
        }

        private void LogError(HttpContext context, Exception ex, double elapsedMilliseconds)
        {
            using (LogContext.PushProperty("RequestPath", context.Request.Path.ToString()))
            using (LogContext.PushProperty("RequestMethod", context.Request.Method))
            using (LogContext.PushProperty("StatusCode", context.Response.StatusCode))
            using (LogContext.PushProperty("ElapsedMilliseconds", elapsedMilliseconds))
            using (LogContext.PushProperty("User", context.User?.Identity?.Name ?? "Anonymous"))
            using (LogContext.PushProperty("TraceId", context.TraceIdentifier))
            using (LogContext.PushProperty("SpanId", Activity.Current?.SpanId.ToString() ?? "N/A"))
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
            }
        }
    }
}
