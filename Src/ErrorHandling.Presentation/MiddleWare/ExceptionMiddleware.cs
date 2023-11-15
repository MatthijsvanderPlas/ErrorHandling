using System.Net;
using System.Text.Json;
using ErrorHandling.Domain.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using ILogger = Serilog.ILogger;

namespace ErrorHandling.Presentation.MiddleWare;

public class ExceptionMiddleware(ILogger logger) : IFunctionsWorkerMiddleware
{
    readonly ILogger _logger = logger;

    public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception e)
        {
            _logger.Error(e, "Error");
            if (e is ValidationException validationException)
                await HandleValidationExceptionAsync(context, validationException);
            if (e is IDomainException domainException)
                await HandleDomainExceptionAsync<IDomainException>(context, domainException);
            
            await HandleExceptionAsync(context, e);
        } 
    }
    
    static async Task HandleValidationExceptionAsync(FunctionContext context, ValidationException 
        exception)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData.CreateResponse();
        response.StatusCode = HttpStatusCode.BadRequest;
        await response.WriteAsJsonAsync(new {message = exception.Message});
        var invocationResult = context.GetInvocationResult();
        invocationResult.Value = response;
    }
    
    static async Task HandleDomainExceptionAsync<T>(FunctionContext context, T exception) where T : 
        IDomainException
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData.CreateResponse();
        response.StatusCode = HttpStatusCode.Conflict;
        var ex = exception as Exception;
        await response.WriteAsJsonAsync(new {message = ex.Message});
        var invocationResult = context.GetInvocationResult();
        invocationResult.Value = response;
    }
    
    static async Task HandleExceptionAsync(FunctionContext context, Exception exception)
    {
        var requestData = await context.GetHttpRequestDataAsync();
        var response = requestData.CreateResponse();
        response.StatusCode = HttpStatusCode.InternalServerError;
        await response.WriteAsJsonAsync(new {message = exception.Message});
        var invocationResult = context.GetInvocationResult();
        invocationResult.Value = response;
    }
}