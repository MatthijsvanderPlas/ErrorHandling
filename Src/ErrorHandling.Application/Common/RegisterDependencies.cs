using Microsoft.Extensions.DependencyInjection;

namespace ErrorHandling.Application.Common;

public static class RegisterDependencies
{
    public static IServiceCollection AddApplicationError(this IServiceCollection services)
    {
        services.AddMediator(options =>
        {
            options.ServiceLifetime = ServiceLifetime.Scoped;
        });

        return services;
    }
}