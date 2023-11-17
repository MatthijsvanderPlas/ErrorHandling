using System.Net;
using ErrorHandling.Application.Todos.CreateTodo;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ErrorHandling.Presentation.Functions;

public partial class Todos
{
    [Function("CreateTodo")]
    [OpenApiOperation(operationId: "CreateTodo", tags: new[] { "todos" }, Summary = "Create a new todo")]
    [OpenApiParameter("title", In = ParameterLocation.Query, Type = typeof(string), Required = 
        true)]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Function, "post", Route = "todos")] HttpRequestData req,
        FunctionContext executionContext, string title)
    {
        var logger = executionContext.GetLogger("CreateTodo");
        
        logger.LogInformation("C# HTTP trigger function processed a request.");
        var request = new CreateTodoRequest(title);
        var result = await _sender.Send(new CreateTodoCommand(request));
        
        var response = req.CreateResponse(HttpStatusCode.Created);
        await response.WriteAsJsonAsync(result.Id);
        return response;
    }
}
