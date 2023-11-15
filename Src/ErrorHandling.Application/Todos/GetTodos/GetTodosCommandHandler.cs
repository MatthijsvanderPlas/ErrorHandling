using ErrorHandling.Domain.Entity;
using ErrorHandling.Domain.Interfaces;
using Mediator;

namespace ErrorHandling.Application.Todos.GetTodos;

public class GetTodosCommandHandler(ITodoRepository todoRepository) : ICommandHandler<GetTodosCommand, List<Todo>>
{
    public async ValueTask<List<Todo>> Handle(GetTodosCommand command, CancellationToken cancellationToken)
    {
        return await todoRepository.GetAllAsync();
    }
}