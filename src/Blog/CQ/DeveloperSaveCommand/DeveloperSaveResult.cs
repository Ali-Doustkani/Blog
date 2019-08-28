using Blog.Domain.DeveloperStory;

namespace Blog.CQ.DeveloperSaveCommand
{
   public class DeveloperSaveResult
   {
      public DeveloperSaveResult(bool created, DeveloperUpdateCommandResult updateResult)
      {
         Created = created;
         UpdateResult = updateResult;
      }

      public bool Created { get; }
      public DeveloperUpdateCommandResult UpdateResult { get; }
   }
}
