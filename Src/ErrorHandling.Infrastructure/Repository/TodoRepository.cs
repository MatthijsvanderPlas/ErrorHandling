using ErrorHandling.Domain.Entity;
using ErrorHandling.Domain.Exceptions;
using ErrorHandling.Domain.Interfaces;
using ErrorHandling.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

namespace ErrorHandling.Infrastructure.Repository;

public class TodoRepository(TodoContext context) : ITodoRepository
{
    readonly ILogger _logger = Log.ForContext<TodoRepository>();

    public async Task<List<Todo>> GetAllAsync()
    {
        try
        {
            var result = await context.Todos.ToListAsync();
            return result;
        }
        catch (Exception e)
        {
            _logger.Error(e, "Database unreachable");
            throw;
        }
    }

    public async Task<Todo> GetByIdAsync(int id)
    {
        _logger.Information("Getting Todo by id");
        var result = await context.Todos.FindAsync(id);
        if (result is null)
        {
            _logger.Error("Todo not found");
            throw new NoTodoFoundException();
        }
        return result;
    }

    public async Task<Todo> CreateAsync(string todo)
    {
        _logger.Information("Creating Todo");
        var newTodo = Todo.Create(todo);
        await context.Todos.AddAsync(newTodo);
        return newTodo;
    }

    public async Task DeleteAsync(int id)
    {
        _logger.Information("Deleting Todo");
        var todo = await context.Todos.FindAsync(id);
        if (todo is null)
        {
            _logger.Error("Todo not found");
            throw new NoTodoFoundException(); 
        }
        context.Todos.Remove(todo);
    }
}