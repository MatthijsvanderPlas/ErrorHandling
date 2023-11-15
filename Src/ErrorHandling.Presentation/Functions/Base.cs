using Mediator;
using ILogger = Serilog.ILogger;

namespace ErrorHandling.Presentation.Functions;

public partial class Todos
{
   readonly ILogger _logger;
   readonly ISender _sender;

   public Todos(ILogger logger, ISender sender)
   {
      _logger = logger;
      _sender = sender;
   }
}