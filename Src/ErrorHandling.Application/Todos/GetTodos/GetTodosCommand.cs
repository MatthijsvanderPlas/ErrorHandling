using ErrorHandling.Domain.Entity;
using Mediator;

namespace ErrorHandling.Application.Todos.GetTodos;

public record GetTodosCommand() : ICommand<List<Todo>>;