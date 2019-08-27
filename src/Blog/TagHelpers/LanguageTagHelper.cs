using Blog.Domain;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Linq;

namespace Blog.TagHelpers
{
   [HtmlTargetElement(Attributes = "blog-bilingual")]
   public class LanguageTagHelper : TagHelper
   {
      public bool BlogBilingual { get; set; }

      [HtmlAttributeNotBound]
      [ViewContext]
      public ViewContext ViewContext { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         if (BlogBilingual && Equals(ViewContext.ViewData["language"], Language.Farsi))
         {
            if (output.Attributes.ContainsName("class"))
            {
               var classAttrib = output.Attributes.Single(x => x.Name == "class");
               var newValue = $"{classAttrib.Value} farsi";
               output.Attributes.Remove(classAttrib);
               output.Attributes.SetAttribute("class", newValue);
            }
         }
      }
   }
}
