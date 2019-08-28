using MediatR;

namespace Blog.CQ.DraftDeleteCommand
{
   public class DraftDeleteCommand : IRequest
   {
      public int Id { get; set; }
   }
}
