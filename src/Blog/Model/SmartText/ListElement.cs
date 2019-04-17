using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Model.SmartText
{
    public class ListElement : IElement
    {
        public ListElement(IEnumerable<string> items)
        {
            Items = items.ToArray();
        }

        public IEnumerable<string> Items { get; }

        public override bool Equals(object obj)
        {
            var other = obj as ListElement;
            if (other == null) return false;
            if (other.Items.Count() != Items.Count()) return false;
            for (int i = 0; i < other.Items.Count(); i++)
                if (other.Items.ElementAt(i) != Items.ElementAt(i))
                    return false;
            return true;
        }

        public override int GetHashCode() => HashCode.Combine(Items);
    }
}
