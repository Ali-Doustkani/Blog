using System;
using System.Collections.Generic;

namespace Blog.Validation
{
   public class Property
   {
      public Property(IEnumerable<Attribute> attributes, object value, IEnumerable<object> path)
      {
         Attributes = attributes;
         Value = value;
         Path = path;
      }

      public IEnumerable<Attribute> Attributes { get; }
      public object Value { get; }
      public IEnumerable<object> Path { get; }
   }
}
