using System.Net;
using ErrorHandling.Domain.Primitives;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Serilog;
using Serilog.Core;
using ILogger = Serilog.ILogger;

namespace ErrorHandling.Presentation.MiddleWare;

public class ExceptionMiddleware : IFunctionsWorkerMiddleware
{
    readonly ILogger _logger = Log.ForContext<ExceptionMiddleware>();

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e) when (e.InnerException is ValidationException exception)
        {
            _logger.Error(exception, "Error: {Message}", exception.Message);
            await HandleValidationExceptionAsync(context, exception);
        }
        catch (Exception e) when (e.InnerException is DomainException exception)
        {
            _logger.Error(exception, "Error: {Message}", e.Message);

            await HandleDomainExceptionAsync(context, exception);

        }
        catch (Exception e) when (e is FunctionWorkerException exception)
        {
            _logger.Error(exception, "Error: {Message}", e.Message);
            await HandleBadRequestExceptionAsync(context, exception);
        }
        catch (Exception e) when (e is SystemException exception)
        {
            _logger.Error(exception, "Error: {Message}", e.Message);
            await HandleBadRequestExceptionAsync(context, exception);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error Basic Exception: {Message}", e.Message);

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
            Title = exception.InnerException?.Message ?? "Validation Error",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Instance = context.InvocationId
        };
        
        await response.WriteAsJsonAsync(details, response.StatusCode);
        context.GetInvocationResult().Value = response;
    }
    
    static async Task HandleBadRequestExceptionAsync(FunctionContext context, Exception 
        exception)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData.CreateResponse(HttpStatusCode.BadRequest);
        
        var details = new ProblemDetails()
        {
            Detail = exception.Message,
            Status = (int)HttpStatusCode.BadRequest,
            Title = "Bad Request", 
            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
            Instance = context.InvocationId
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
            Detail = exception.StackTrace,
            Status = (int)exception.StatusCode,
            Title = exception.Title,
            Type = exception.Type,
            Instance = context.InvocationId
        };

        await response.WriteAsJsonAsync(details, response.StatusCode);
        context.GetInvocationResult().Value = response;
    }

    static async Task HandleExceptionAsync(FunctionContext context, Exception exception)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData!.CreateResponse(HttpStatusCode.InternalServerError);

        var details = new ProblemDetails()
        {
            Detail = exception.Message,
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Internal Server Error",
            Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
            Instance = context.InvocationId
        };

        await response.WriteAsJsonAsync(details, response.StatusCode);
        context.GetInvocationResult().Value = response;
    }
}