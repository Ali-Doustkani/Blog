using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blog.Validation
{
   public class PropertyNavigator
   {
      public PropertyNavigator(object model)
      {
         _path = new Stack<object>();
         _iterator = new IteratorStack();
         _iterator.PushObject(model);
      }

      private readonly IteratorStack _iterator;
      private readonly Stack<object> _path;
      private int _index;

      public Property Property { get; private set; }

      public void CheckForEachAttribute(Action<Attribute, Property> action)
      {
         foreach (var attrib in Property.Attributes)
            action(attrib, Property);
      }

      public bool Read()
      {
         if (!_iterator.Any())
            return false;

         if (_iterator.MoveNext())
         {
            if (HasAttribute())
            {
               MakeProperty();
               StepInIfClass();
               return true;
            }
            StepInIfClass();
            return Read();
         }
         else
         {
            StepOut();
            return Read();
         }
      }

      private bool HasAttribute() =>
        _iterator.Current is PropertyInfo && _iterator.CurrentProperty.GetCustomAttributes().Any();

      private void MakeProperty()
      {
         var path = _path.Reverse().Concat(new[] { CamelCase(_iterator.CurrentProperty.Name) }).ToArray();
         Property = new Property(_iterator.CurrentProperty.GetCustomAttributes(),
            _iterator.CurrentProperty.GetValue(_iterator.CurrentOwner),
            path);
      }

      private void StepInIfClass()
      {
         if (_iterator.Current is PropertyInfo &&
            (_iterator.CurrentProperty.PropertyType == typeof(string) || _iterator.CurrentProperty.PropertyType.IsPrimitive))
            return;

         if (IsCollection())
         {
            _path.Push(CamelCase(_iterator.CurrentProperty.Name));
            _iterator.PushCurrentAsCollection();
         }
         else if (_iterator.Current is PropertyInfo)
         {
            _path.Push(CamelCase(_iterator.CurrentProperty.Name));
            _iterator.PushCurrentAsObject();
         }
         else
         {
            _path.Push(_index);
            _index++;
            _iterator.PushObject(_iterator.Current);
         }
      }

      private bool IsCollection() =>
        _iterator.Current is PropertyInfo && typeof(IEnumerable).IsAssignableFrom(_iterator.CurrentProperty.PropertyType);

      private string CamelCase(string name) =>
         char.ToLowerInvariant(name[0]) + name.Substring(1);

      private void StepOut()
      {
         _iterator.Pop();
         if (_path.Any())
            _path.Pop();
      }
   }
}
