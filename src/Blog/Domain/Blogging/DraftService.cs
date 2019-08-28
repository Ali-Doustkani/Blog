using Blog.Infra;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.Blogging
{
   public class DraftSaveResult
   {
      public DraftSaveResult(IEnumerable<Error> errors)
      {
         Errors = errors;
      }

      public DraftSaveResult(Draft draft)
      {
         Errors = Enumerable.Empty<Error>();
         Draft = draft;
      }

      public IEnumerable<Error> Errors { get; }
      public Draft Draft { get; }

      public bool Failed => Errors.Any();
   }

   public class DraftService
   {
      public DraftService(IStorageState storageState, IQueryHandler handler)
      {
         _storageState = storageState;
         _handler = handler;
      }

      private readonly IStorageState _storageState;
      private readonly IQueryHandler _handler;

      public DraftSaveResult Save(DraftUpdateCommand command)
      {
         Draft draft;
         if (command.Id == 0)
         {
            if (_handler.Any(new DraftTitleQuery(command.Id, command.Title)))
               return new DraftSaveResult(new[] { new Error(nameof(draft.Title), "This title already exists in the database") });
            draft = new Draft();
            _storageState.Add(draft);
         }
         else
         {
            draft = _handler.Execute(new DraftIdQuery(command.Id));
         }

         draft.Update(command, _storageState);

         return new DraftSaveResult(draft);
      }
   }
}
