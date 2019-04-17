using System;

namespace Blog.Model.SmartText
{
    public enum NoteType
    {
        Green,
        Red
    }

    public class NoteElement : IElement
    {
        public NoteElement(NoteType type, string value)
        {
            Type = type;
            Value = value;
        }

        public NoteType Type { get; }
        public string Value { get; }

        public override bool Equals(object obj)
        {
            var other = obj as NoteElement;
            if (other == null) return false;
            return other.Type == Type && other.Value == Value;
        }

        public override int GetHashCode() => HashCode.Combine(Type, Value);
    }
}
