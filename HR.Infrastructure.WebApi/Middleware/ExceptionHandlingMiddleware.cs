using System.Net;
using System.Text.Json;
using HR.Application.Dto.Dto;
using HR.Application.Utilities.Utils;
using HR.Infrastructure.WebApi.Middleware.Exceptions;
using KeyNotFoundException = HR.Infrastructure.WebApi.Middleware.Exceptions.KeyNotFoundException;
using NotImplementedException = HR.Infrastructure.WebApi.Middleware.Exceptions.NotImplementedException;
using UnauthorizedAccessException = HR.Infrastructure.WebApi.Middleware.Exceptions.UnauthorizedAccessException;

namespace HR.Infrastructure.WebApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;

            var errorResponse = new TemplateApi<UserDto>(null, null, "Đã xảy ra lỗi !", false, 0, 0, 0, 0);

            var exceptionType = exception.GetType();
            if (exceptionType == typeof(BadRequestException))
            {
                response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else if (exceptionType == typeof(NotFoundException))
            {
                response.StatusCode = (int)HttpStatusCode.NotFound;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                response.StatusCode = (int)HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(UnauthorizedAccessException))
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(KeyNotFoundException))
            {
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
            }
            else
            {
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
            }

            _logger.LogError("Đã xảy ra lỗi - {exception}", exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}
