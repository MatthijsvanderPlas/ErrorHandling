using ErrorHandling.Domain.Interfaces;

namespace ErrorHandling.Domain.Exceptions;

public class NoTodoFoundException : Exception, IDomainException
{
    public NoTodoFoundException() : base("Todo not found")
    {
    }
}