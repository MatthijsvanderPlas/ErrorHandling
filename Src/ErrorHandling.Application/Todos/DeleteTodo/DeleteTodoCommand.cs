using Mediator;

namespace ErrorHandling.Application.Todos.DeleteTodo;

public record DeleteTodoCommand(Guid Id) : ICommand;
    