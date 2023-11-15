using ErrorHandling.Domain.Interfaces;
using ErrorHandling.Infrastructure.Persistence;
using ErrorHandling.Infrastructure.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ErrorHandling.Infrastructure.Common;

public static class RegisterDependencies
{
    public static IServiceCollection AddInfrastructureError(this IServiceCollection services)
    {
        services.AddDbContext<TodoContext>();

        services.AddScoped<ITodoRepository, TodoRepository>();

        return services;
    }
}