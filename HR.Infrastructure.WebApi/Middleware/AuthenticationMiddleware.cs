using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;
using HR.Application.Dto.Dto;
using HR.Application.Interface.Interfaces;
using HR.Application.Utilities.Utils;
using HR.Domain.Kernel.Utils;
using Microsoft.IdentityModel.Tokens;

namespace HR.Infrastructure.WebApi.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthenticationMiddleware> _logger;
        private readonly IUserRepository _userRepository;

        public AuthenticationMiddleware(RequestDelegate next, ILogger<AuthenticationMiddleware> logger,
            IUserRepository userRepository)
        {
            _next = next;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var jwt = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var userEmail = ValidateToken(jwt ?? "");

                var user = await _userRepository.UserByEmail(userEmail ?? "");
                if (user?.Id is not null && user.Id != Guid.Empty)
                {
                    httpContext.Items["UserId"] = user.Id;
                    httpContext.Items["UserName"] = user.Fullname;
                }

                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private static string? ValidateToken(string token)
        {
            if (token == "")
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppSettings.SecretKey);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userEmail = jwtToken.Claims.First(x => x.Type == "email").Value;

                return userEmail;
            }
            catch
            {
                return null;
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = context.Response;
            response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errorResponse = new TemplateApi<UserDto>(null, null, "Đã xảy ra lỗi !", false, 0, 0, 0, 0);

            _logger.LogError("Đã xảy ra lỗi - {exception}", exception.Message);
            var result = JsonSerializer.Serialize(errorResponse);
            await context.Response.WriteAsync(result);
        }
    }
}