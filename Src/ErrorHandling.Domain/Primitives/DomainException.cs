using System.Net;

namespace ErrorHandling.Domain.Primitives;

public class DomainException : Exception
{
   public string Type { get; set; } = string.Empty;
   public string Title { get; set; } = string.Empty;
   public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.InternalServerError;
   
   public DomainException(string message) : base(message)
   {
      
   } 
}