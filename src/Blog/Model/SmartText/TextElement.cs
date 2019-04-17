using System;

namespace Blog.Model.SmartText
{
    public enum TextType
    {
        SmallHeader,
        LargeHeader,
        Paragraph
    }

    public class TextElement : IElement
    {
        public TextElement(TextType type, string value)
        {
            Type = type;
            Value = value;
        }

        public TextType Type { get; }
        public string Value { get; }

        public override bool Equals(object obj)
        {
            var other = obj as TextElement;
            if (other == null) return false;
            return other.Type == Type && other.Value == Value;
        }

        public override int GetHashCode() => HashCode.Combine(Type, Value);
    }
}
