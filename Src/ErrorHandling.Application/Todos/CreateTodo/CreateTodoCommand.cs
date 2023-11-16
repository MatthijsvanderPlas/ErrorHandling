using ErrorHandling.Domain.Entity;
using Mediator;

namespace ErrorHandling.Application.Todos.CreateTodo;

public record CreateTodoCommand(CreateTodoRequest TodoRequest) : ICommand<Todo>;
    