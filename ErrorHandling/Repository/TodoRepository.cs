using ErrorHandling.Entity;
using ErrorHandling.Persistence;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace ErrorHandling.Repository;

public class TodoRepository
{
    private readonly TodoContext _context;

    public TodoRepository(TodoContext context)
    {
        _context = context;
    }
    
    public async Task<Result<List<Todo>>> GetAllAsync()
    {
        var result = await _context.Todos.ToListAsync();
        if (result.Count == 0)
        {
            return Result.Fail("Todos not found");
        }
        return Result.Ok(result);
    }
    
    public async Task<Result<Todo>> GetByIdAsync(int id)
    {
        var result = await _context.Todos.FindAsync(id);
        if (result is null)
        {
            throw new Exception("Todo not found");
        }
        return Result.Ok(result);
    }
    
    public async Task<Result<Todo>> CreateAsync(Todo todo)
    {
        await _context.Todos.AddAsync(todo);
        await _context.SaveChangesAsync();
        return Result.Ok(todo);
    }
    
    public async Task<Result<Todo>> UpdateAsync(Todo todo)
    {
        _context.Todos.Update(todo);
        await _context.SaveChangesAsync();
        return Result.Ok(todo);
    }
    
    public async Task<Result> DeleteAsync(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo is null)
        {
            return Result.Fail("Todo not found");
        }
        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();
        return Result.Ok();
    }
    
    
}