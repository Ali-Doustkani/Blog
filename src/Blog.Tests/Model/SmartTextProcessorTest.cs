using Blog.Model.SmartText;
using Xunit;

namespace Blog.Tests
{
    public class SmartTextProcessorTest
    {
        private void Parse(string smartText, params IElement[] nodes)
        {
            var processor = new SmartTextProcessor();
            Assert.Equal(nodes, processor.Process(smartText));
        }

        [Fact]
        public void Parse_title() => Parse(
            "# title",
            new TextElement(TextType.LargeHeader, "title")
            );

        [Fact]
        public void Parse_titles() => Parse(
            "#big title\n## small title",
            new TextElement(TextType.LargeHeader, "big title"),
            new TextElement(TextType.SmallHeader, "small title")
            );

        [Fact]
        public void Parse_title_and_paragraph() => Parse(
            "#Programming with java\nread code to learn java",
            new TextElement(TextType.LargeHeader, "Programming with java"),
            new TextElement(TextType.Paragraph, "read code to learn java")
            );

        [Fact]
        public void Parse_codes() => Parse(
            "some paragraph\n//cs\nvar a = new Array();\n//\n//js\nlet a = 12;\n//",
            new TextElement(TextType.Paragraph, "some paragraph"),
            new CodeElement(LanguageType.CSharp, "var a = new Array();"),
            new CodeElement(LanguageType.Javascript, "let a = 12;")
            );

        [Fact]
        public void Parse_list_items() => Parse(
            ". first item\n.second item\n. third item",
            new ListElement(new[] { "first item", "second item", "third item" })
            );

        [Fact]
        public void Parse_list_and_paragraphs() => Parse(
            "paragraph1\n.item a1\n. item a2\nparagraph2\n.item b1\n.item b2",
            new TextElement(TextType.Paragraph, "paragraph1"),
            new ListElement(new[] { "item a1", "item a2" }),
            new TextElement(TextType.Paragraph, "paragraph2"),
            new ListElement(new[] { "item b1", "item b2" })
            );

        [Fact]
        public void Ignore_white_spaces() => Parse(
            "# \n## \n paragraph\n\n\n#head",
            new TextElement(TextType.Paragraph, "paragraph"),
            new TextElement(TextType.LargeHeader, "head")
            );

        [Fact]
        public void Parse_note_boxes() => Parse(
            "! some green note\n!! some red note!",
            new NoteElement(NoteType.Green, "some green note"),
            new NoteElement(NoteType.Red, "some red note!")
            );
    }
}
