using Blog.Model;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Blog.TagHelpers
{
    public class LanguageTagHelper : TagHelper
    {
        public LanguageTagHelper()
        {
            Is = Language.Farsi;
        }

        public Language Is { get; set; }

        public object Value { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = null;
            if (Value is Language langValue && Is != langValue)
                output.SuppressOutput();
        }
    }
}
