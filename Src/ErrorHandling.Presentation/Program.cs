using ErrorHandling.Application.Common;
using ErrorHandling.Infrastructure.Common;
using ErrorHandling.Presentation.MiddleWare;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.ApplicationInsights(TelemetryConverter.Traces)
    .WriteTo.Console()
    .CreateLogger();

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults(options =>
    {
        options.UseMiddleware<ExceptionMiddleware>();
    })
    .ConfigureServices(services =>
    {
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(Log.Logger, true));
        services.AddApplicationError();
        services.AddInfrastructureError();
    })
    .Build();

host.Run();