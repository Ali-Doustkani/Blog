using MediatR;

namespace Blog.CQ.PostQuery
{
   public class PostQuery : IRequest<DraftSaveCommand.DraftSaveCommand>
   {
      public int Id { get; set; }
   }
}
