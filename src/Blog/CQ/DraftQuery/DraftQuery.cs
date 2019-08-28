using MediatR;

namespace Blog.CQ.DraftQuery
{
   public class DraftQuery : IRequest<DraftSaveCommand.DraftSaveCommand>
   {
      public int Id { get; set; }
   }
}
