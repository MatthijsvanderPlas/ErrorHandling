using System.Net;
using ErrorHandling.Application.Todos.DeleteTodo;
using FluentValidation;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace ErrorHandling.Presentation.Functions;

public partial class Todos 
{
   [Function("DeleteTodo")]
   [OpenApiOperation("DeleteTodo", "todos", Summary = "Delete a todo")]
   [OpenApiParameter("id", In = ParameterLocation.Path, Type = typeof(Guid), Required = true)]
   
   public async Task<HttpResponseData> DeleteTodo(
       [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "todos/{id}")] HttpRequestData req,
       FunctionContext executionContext, Guid id)
   {
       var logger = executionContext.GetLogger("DeleteTodo");
       logger.LogInformation("C# HTTP trigger function processed a request.");
       // var todoId = Guid.TryParse(id, out var guid) ? guid : throw new ValidationException("Guid is not valid");
       await _sender.Send(new DeleteTodoCommand(id));
       var response = req.CreateResponse(HttpStatusCode.NoContent);
       return response;
   }
}