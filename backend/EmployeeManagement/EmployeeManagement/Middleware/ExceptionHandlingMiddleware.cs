using EmployeeManagement.Domain.Exceptions;
using System.Net;
using System.Text.Json;

namespace EmployeeManagement.Api.Middleware
{

    /// <summary>
    /// Global exception handling middleware
    /// Catches all unhandled exceptions and returns appropriate HTTP responses
    /// </summary>
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Call next middleware in pipeline
                await _next(context);
            }
            catch (Exception ex)
            {
                // Log the exception
                _logger.LogError(ex, "An unhandled exception occurred: {Message}", ex.Message);

                // Handle the exception
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Set response content type
            context.Response.ContentType = "application/json";

            // Determine status code and message based on exception type
            var (statusCode, message) = exception switch
            {
                NotFoundException notFoundEx =>
                    (HttpStatusCode.NotFound, notFoundEx.Message),

                ValidationException validationEx =>
                    (HttpStatusCode.BadRequest, validationEx.Message),

                BusinessException businessEx =>
                    (HttpStatusCode.BadRequest, businessEx.Message),

                _ =>
                    (HttpStatusCode.InternalServerError, "An error occurred while processing your request.")
            };

            // Set status code
            context.Response.StatusCode = (int)statusCode;

            // Create error response object
            var errorResponse = new ErrorResponse
            {
                Message = message,
                StatusCode = (int)statusCode,
                Timestamp = DateTime.UtcNow
            };

            // Serialize with explicit options
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true  // Makes it readable
            };

            var json = JsonSerializer.Serialize(errorResponse, options);

            await context.Response.WriteAsync(json);
        }
    }

    /// <summary>
    /// Standard error response format
    /// </summary>
    public class ErrorResponse
    {
        public string Message { get; set; } = string.Empty;
        public int StatusCode { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
