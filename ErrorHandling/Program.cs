using System.Net;
using ErrorHandling.Entity;
using ErrorHandling.Persistence;
using ErrorHandling.Repository;
using Microsoft.EntityFrameworkCore;
using Serilog;

var logger = new LoggerConfiguration()
    .WriteTo.ApplicationInsights(TelemetryConverter.Traces)
    .WriteTo.Console()
    .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoContext>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddScoped<TodoRepository>();
builder.Host.UseSerilog(logger);
var app = builder.Build();

app.Use(async (context, next) =>
{
    try
    {
        await next.Invoke(context);
    }
    catch (Exception e)
    {
        logger.Error("Unexpected Error: {Error}",e.ToString());
        if (!context.Response.HasStarted)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(e.Message);
        }
        else
        {
            await context.Response.WriteAsync(e.Message);
        }
    }
    
});

app.MapGet("/", async (TodoRepository repository) =>
{
    var result = repository.GetAllAsync().Result;
    if (result.IsFailed)
    {
        return Results.Problem(result.Errors.First().Message, 
            statusCode: (int?)HttpStatusCode.InternalServerError);
    }
    return Results.Ok(result.Value);
});

app.MapGet("/{id}", async (TodoRepository repository, int id) =>
{
    var result = repository.GetByIdAsync(id).Result;
    if (result.IsFailed)
    {
        return Results.Problem(result.Errors.First().Message);
    }
    return Results.Ok(result.Value);
});

app.MapPost("/", async (TodoRepository repository, Todo todo) =>
{
    var result = repository.CreateAsync(todo).Result;
    if (result.IsFailed)
    {
        return Results.Problem(result.Errors.First().Message);
    }
    return Results.Ok(result.Value);
});

app.Run();