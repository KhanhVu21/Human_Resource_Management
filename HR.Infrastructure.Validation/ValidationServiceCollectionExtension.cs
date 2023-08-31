using FluentValidation;
using HR.Infrastructure.Validation.Models.User;
using HR.Infrastructure.Validation.Validation.User;
using Microsoft.Extensions.DependencyInjection;

namespace HR.Infrastructure.Validation;

public static class ValidationServiceCollectionExtension
{
    public static void ValidationRegisterServices(this IServiceCollection services)
    {
        // Register validator with service provider (or use one of the automatic registration methods)
        services.AddTransient<IValidator<UserRegisterRequest>, UserRegisterRequestValidator>();
        services.AddTransient<IValidator<UserLoginRequest>, UserLoginRequestValidator>();

        services.AddTransient<ValidatorProvider>();
    }
}