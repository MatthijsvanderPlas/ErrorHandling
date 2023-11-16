using System.Net;
using ErrorHandling.Domain.Primitives;

namespace ErrorHandling.Domain.Exceptions;

public class NoTodoFoundException : DomainException
{
    public NoTodoFoundException() : base("Todo not found")
    {
        Title = "Todo not found";
        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4";
        StatusCode = HttpStatusCode.NotFound;
    }
}