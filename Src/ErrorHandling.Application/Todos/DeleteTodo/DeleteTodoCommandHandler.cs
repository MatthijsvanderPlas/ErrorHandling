using ErrorHandling.Domain.Interfaces;
using ErrorHandling.Infrastructure.Persistence;
using Mediator;

namespace ErrorHandling.Application.Todos.DeleteTodo;

public class DeleteTodoCommandHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork) : 
    ICommandHandler<DeleteTodoCommand>
{

    public ValueTask<Unit> Handle(DeleteTodoCommand command, CancellationToken cancellationToken)
    {
        todoRepository.DeleteAsync(command.Id);
        unitOfWork.SaveChangesAsync();
        return Unit.ValueTask;
    }
}