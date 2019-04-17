using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Model.SmartText
{
    public class SmartTextProcessor
    {
        private List<IElement> result;
        private List<string> listItemsBuffer;

        public IEnumerable<IElement> Process(string smartText)
        {
            result = new List<IElement>();
            listItemsBuffer = new List<string>();

            var lines = smartText.Split('\n').GetEnumerator();

            while (lines.MoveNext())
            {
                var line = (string)lines.Current;

                if (line.StartsWith("."))
                {
                    BufferItem(line);
                    continue;
                }

                FlushBufferIfAny();

                if (line.StartsWith("##"))
                    AddIf(val => result.Add(new TextElement(TextType.SmallHeader, val)), Ignore(line, 2));
                else if (line.StartsWith("#"))
                    AddIf(val => result.Add(new TextElement(TextType.LargeHeader, val)), Ignore(line, 1));
                else if (line.StartsWith("//"))
                    Code(lines, line);
                else if (line.StartsWith("!!"))
                    AddIf(val => result.Add(new NoteElement(NoteType.Red, val)), Ignore(line, 2));
                else if (line.StartsWith("!"))
                    AddIf(val => result.Add(new NoteElement(NoteType.Green, val)), Ignore(line, 1));
                else
                    AddIf(val => result.Add(new TextElement(TextType.Paragraph, val)), line.Trim());
            }

            FlushBufferIfAny();

            return result;
        }

        private void BufferItem(string line)
        {
            listItemsBuffer.Add(Ignore(line, 1));
        }

        private void FlushBufferIfAny()
        {
            if (!listItemsBuffer.Any()) return;
            result.Add(new ListElement(listItemsBuffer));
            listItemsBuffer.Clear();
        }

        private void Code(IEnumerator lines, string line)
        {
            var strLang = Ignore(line, 2).Trim();
            var lang = LanguageType.None;
            if (strLang == "cs")
                lang = LanguageType.CSharp;
            else if (strLang == "js")
                lang = LanguageType.Javascript;
            else if (strLang == "cmd")
                lang = LanguageType.Command;

            var code = string.Empty;
            lines.MoveNext();
            while (!lines.Current.Equals("//"))
            {
                code += lines.Current;
                lines.MoveNext();
            }
            result.Add(new CodeElement(lang, code));
        }

        private void AddIf(Action<string> add, string content)
        {
            if (!string.IsNullOrWhiteSpace(content))
                add(content);
        }

        private string Ignore(string input, int count) => input.Substring(count, input.Length - count).Trim();
    }
}
