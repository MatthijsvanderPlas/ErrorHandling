using ErrorHandling.Domain.Entity;
using ErrorHandling.Domain.Interfaces;
using ErrorHandling.Infrastructure.Persistence;
using Mediator;

namespace ErrorHandling.Application.Todos.CreateTodo;

public class CreateTodoCommandHandler(ITodoRepository todoRepository, IUnitOfWork unitOfWork) : ICommandHandler<CreateTodoCommand, Todo>
{
    public async ValueTask<Todo> Handle(CreateTodoCommand command, CancellationToken cancellationToken)
    {
        var todo = await todoRepository.CreateAsync(command.TodoRequest.Title);
        await unitOfWork.SaveChangesAsync();
        return todo;
    }
}