using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Blog.Validation
{
   public class IteratorStack : IEnumerator
   {
      public IteratorStack()
      {
         _pool = new Stack<Tuple<object, IEnumerator>>();
      }

      private Stack<Tuple<object, IEnumerator>> _pool;

      private IEnumerator iterator => _pool.Peek().Item2;

      public object Current => iterator.Current;
      public PropertyInfo CurrentProperty { get => (PropertyInfo)iterator.Current; }
      public object CurrentOwner { get => _pool.Peek().Item1; }

      public void PushCollection(IEnumerable collection) =>
         _pool.Push(new Tuple<object, IEnumerator>(collection, collection.GetEnumerator()));

      public void PushObjectType(object model, Type type) =>
         _pool.Push(new Tuple<object, IEnumerator>(model, type.GetProperties().GetEnumerator()));

      public void PushObject(object model) =>
         _pool.Push(new Tuple<object, IEnumerator>(model, model.GetType().GetProperties().GetEnumerator()));

      public void Pop() => _pool.Pop();

      public bool Any() => _pool.Any();

      public bool MoveNext() => iterator.MoveNext();

      public void Reset() => iterator.Reset();
   }
}
