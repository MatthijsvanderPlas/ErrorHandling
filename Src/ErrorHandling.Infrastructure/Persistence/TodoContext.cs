using ErrorHandling.Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace ErrorHandling.Infrastructure.Persistence;

public class TodoContext : DbContext
{
    string _fullPath;
    
    public TodoContext(DbContextOptions<TodoContext> options) : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _fullPath = Path.Combine(path, "Todo.db");
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={_fullPath}");
    
    public DbSet<Todo> Todos => Set<Todo>();
}