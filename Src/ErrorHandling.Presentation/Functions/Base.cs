using Mediator;
using ILogger = Serilog.ILogger;

namespace ErrorHandling.Presentation.Functions;

public partial class Todos
{
   readonly ISender _sender;

   public Todos(ISender sender)
   {
      _sender = sender;
   }
}