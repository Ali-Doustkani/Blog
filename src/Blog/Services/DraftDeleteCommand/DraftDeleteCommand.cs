using MediatR;

namespace Blog.Services.DraftDeleteCommand
{
   public class DraftDeleteCommand : IRequest
   {
      public int Id { get; set; }
   }
}
