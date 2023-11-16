namespace ErrorHandling.Infrastructure.Persistence;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}