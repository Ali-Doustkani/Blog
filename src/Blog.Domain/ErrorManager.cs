using Blog.Domain.DeveloperStory;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class ErrorManager
   {
      public ErrorManager()
      {
         _errors = new List<string>();
      }

      private readonly List<string> _errors;

      public ErrorManager Conditional(Action<ErrorManager> conditional, Action<ErrorManager> then)
      {
         var cond = new ErrorManager();
         conditional(cond);
         if (!cond.Dirty)
            then(cond);
         _errors.AddRange(cond._errors);
         return this;
      }

      public ErrorManager Required(string value, string name)
      {
         if (string.IsNullOrWhiteSpace(value))
            _errors.Add($"'{name}' is required");
         return this;
      }

      public ErrorManager IfTrue(bool value, string message)
      {
         if (value)
            _errors.Add(message);
         return this;
      }

      public ErrorManager Add(string message)
      {
         _errors.Add(message);
         return this;
      }

      public ErrorManager Add(IEnumerable<string> errors)
      {
         _errors.AddRange(errors);
         return this;
      }

      public ErrorManager NotEmpty<T>(IEnumerable<T> list, string listName)
      {
         if (!list.Any())
            _errors.Add($"There should be at least one item in {listName}");
         return this;
      }

      public ErrorManager NoDuplicate<T>(IEnumerable<T> list,
         Func<T, object> equality,
         Func<T, string> messageMaker)
      {
         var dupList = list.GroupBy(equality).Where(x => x.Count() > 1);
         foreach (var item in dupList)
            _errors.Add(messageMaker(item.First()));
         return this;
      }

      public ErrorManager NoOverlaps<T>(IEnumerable<T> list,
         Func<T, string> start,
         Func<T, string> end,
         Func<T, IEnumerable<T>, string> messageMaker)
         where T : class
      {
         var checkedItems = new List<object>();
         var items = list.Select(x => new { Item = x, Period = Period.Parse(start(x), end(x)) });
         foreach (var i in items)
         {
            if (checkedItems.Contains(i))
               continue;

            var overlaps = items.Where(x => x.Item != i.Item && x.Period.Overlaps(i.Period));
            if (overlaps.Any())
            {
               _errors.Add(messageMaker(i.Item, overlaps.Select(x => x.Item)));
               checkedItems.Add(i);
               checkedItems.AddRange(overlaps);
            }
         }
         return this;
      }

      public ErrorManager CheckPeriods<T>(IEnumerable<T> list,
         Func<T, string> start,
         Func<T, string> end,
         Func<T, string> messageMaker)
         where T : class
      {
         foreach (var item in list)
         {
            if (DateTime.Parse(start(item)) >= DateTime.Parse(end(item)))
               _errors.Add(messageMaker(item));
         }
         return this;
      }

      public bool Dirty =>
         _errors.Any();

      public CommandResult ToResult() =>
         new CommandResult(_errors.ToArray());

      public IEnumerable<string> Errors =>
         _errors;
   }
}
