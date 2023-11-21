using ErrorHandling.Domain.Entity;

namespace ErrorHandling.Domain.Interfaces;

public interface ITodoRepository
{
    Task<List<Todo>> GetAllAsync();
    Task<Todo> CreateAsync(string todo);
    Task DeleteAsync(Guid id);
}