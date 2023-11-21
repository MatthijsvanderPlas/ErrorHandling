using ErrorHandling.Application.Common;
using ErrorHandling.Infrastructure.Common;
using ErrorHandling.Presentation.MiddleWare;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(options =>
    {
        options.UseMiddleware<ExceptionMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationError();
        services.AddInfrastructureError();
    })
    .Build();

host.Run();