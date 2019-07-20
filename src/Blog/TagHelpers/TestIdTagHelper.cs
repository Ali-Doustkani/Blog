using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Blog.TagHelpers
{
   [HtmlTargetElement(Attributes = "blog-testid")]
   public class TestIdTagHelper : TagHelper
   {
      public TestIdTagHelper(IHostingEnvironment environment)
      {
         _environment = environment;
      }

      private readonly IHostingEnvironment _environment;

      public string BlogTestid { get; set; }

      public override void Process(TagHelperContext context, TagHelperOutput output)
      {
         if (_environment.IsEnvironment("Testing"))
         {
            output.Attributes.Add("data-cy", BlogTestid);
         }
      }
   }
}
