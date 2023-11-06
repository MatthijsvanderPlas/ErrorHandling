using ErrorHandling.Entity;
using Microsoft.EntityFrameworkCore;

namespace ErrorHandling.Persistence;

public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
    
    public DbSet<Todo> Todos => Set<Todo>();
}