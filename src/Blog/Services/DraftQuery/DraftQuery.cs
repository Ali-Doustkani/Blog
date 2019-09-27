using MediatR;

namespace Blog.Services.DraftQuery
{
   public class DraftQuery : IRequest<DraftSaveCommand.DraftSaveCommand>
   {
      public int Id { get; set; }
   }
}
