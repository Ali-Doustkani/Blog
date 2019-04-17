using System;

namespace Blog.Model.SmartText
{
    public enum LanguageType
    {
        None,
        CSharp, 
        Javascript, 
        Command
    }

    public class CodeElement : IElement
    {
        public CodeElement(LanguageType language, string value)
        {
            Language = language;
            Value = value;
        }

        public LanguageType Language { get; }
        public string Value { get; }

        public override bool Equals(object obj)
        {
            var other = obj as CodeElement;
            if (other == null) return false;
            return other.Value == Value && other.Language == Language;
        }

        public override int GetHashCode() => HashCode.Combine(Value, Language);
    }
}
