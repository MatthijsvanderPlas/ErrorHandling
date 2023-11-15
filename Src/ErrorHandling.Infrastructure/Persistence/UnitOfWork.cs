using Serilog;

namespace ErrorHandling.Infrastructure.Persistence;

public class UnitOfWork(TodoContext context)
{

    public async Task SaveChangesAsync()
    {
        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Log.Error(e, "Database unreachable");
            throw;
        }
        finally
        {
            context.ChangeTracker.Clear();
        }
    }
}