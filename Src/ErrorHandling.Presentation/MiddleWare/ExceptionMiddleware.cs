using System.Net;
using System.Text;
using ErrorHandling.Domain.Primitives;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ErrorHandling.Presentation.MiddleWare;

public class ExceptionMiddleware : IFunctionsWorkerMiddleware
{
    readonly ILogger logger = Log.ForContext<ExceptionMiddleware>();

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e) when (e.InnerException is ValidationException exception)
        {
            logger.Error(exception, "Error: {Message}", exception.Message);
            await HandleValidationExceptionAsync(context, exception);
        }
        catch (Exception e) when (e.InnerException is DomainException exception)
        {
            logger.Error(exception, "Error: {Message}", e.Message);

            await HandleDomainExceptionAsync(context, exception);

        }
        catch (Exception e)
        {
            logger.Error(e, "Error: {Message}", e.Message);

            await HandleExceptionAsync(context, e);
        }
    }

    static async Task HandleValidationExceptionAsync(FunctionContext context, ValidationException
        exception)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData.CreateResponse(HttpStatusCode.BadRequest);
        
        var details = new ProblemDetails()
        {
            Detail = exception.Message,
            Status = (int)HttpStatusCode.BadRequest,
            Title = "Validation Error",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Instance = exception.StackTrace
        };
        
        await response.WriteAsJsonAsync(details, response.StatusCode);
        context.GetInvocationResult().Value = response;
    }

    static async Task HandleDomainExceptionAsync<T>(FunctionContext context, T exception) where T :
        DomainException
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData!.CreateResponse(exception.StatusCode);

        var details = new ProblemDetails()
        {
            Detail = exception.Message,
            Status = (int)exception.StatusCode,
            Title = exception.Title,
            Type = exception.Type,
            Instance = exception.StackTrace
        };

        await response.WriteAsJsonAsync(details);
        context.GetInvocationResult().Value = response;
    }

    static async Task HandleExceptionAsync(FunctionContext context, Exception exception)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData!.CreateResponse(HttpStatusCode.InternalServerError);

        var details = new ProblemDetails()
        {
            Detail = "Something went wrong",
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Internal Server Error",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = exception.StackTrace
        };

        await response.WriteAsJsonAsync(details);
        context.GetInvocationResult().Value = response;
    }
}