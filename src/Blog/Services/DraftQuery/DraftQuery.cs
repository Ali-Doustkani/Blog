using MediatR;

namespace Blog.Services.DraftQuery
{
   public class DraftQuery : IRequest<Draft>
   {
      public int Id { get; set; }
   }
}
