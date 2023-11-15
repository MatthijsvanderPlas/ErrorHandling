using System.Net;
using ErrorHandling.Application.Todos.GetTodos;
using ErrorHandling.Domain.Entity;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;

namespace ErrorHandling.Presentation.Functions;

public partial class Todos
{
    [Function("GetTodos")]
    [OpenApiOperation(operationId: "GetTodos", tags: new[] { "todos" }, Summary = "Get all todos")]
    [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(List<Todo>), Summary = "The response", Description = "This returns the response")]
    [OpenApiResponseWithoutBody(HttpStatusCode.BadRequest, Summary = "Bad request", Description = "Bad request")]
    [OpenApiResponseWithoutBody(HttpStatusCode.Conflict, Summary = "Conflict", Description = "Conflict")]
    
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "todos")] HttpRequestData req,
        FunctionContext executionContext)
    {
        _logger.Information("C# HTTP trigger function processed a request");
        
        var result = await _sender.Send(new GetTodosCommand());
        
        var response = req.CreateResponse(HttpStatusCode.OK);
        await response.WriteAsJsonAsync(result);
        return response;
    }
}