using Blog.Domain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blog.Tests.Domain
{
   public class DraftTests
   {
      private IEnumerable<Image> RenderImages(string html)
      {
         var draft = new Draft();
         draft.Info = new PostInfo { Title = "the post" };
         draft.Content = html;
         return draft.RenderImages();
      }

      #region Publishing

      private string Publish(string html)
      {
         var draft = new Draft();
         draft.Info = new PostInfo { Title = "the post" };
         draft.Content = html;
         draft.RenderImages();
         return draft.Publish().PostContent.Content;
      }

      [Fact]
      public void Return_the_same_for_not_known_elements() =>
          Publish("<span contenteditable=\"true\">TEXT</span><h3>H3</h3><h4>H4</h4>")
          .Should()
          .Be("<span>TEXT</span><h3>H3</h3><h4>H4</h4>");

      [Fact]
      public void Delete_empty_paragraphs() =>
          Publish("<p>1</p><p> </p><p>2</p>")
          .Should()
          .Be("<p>1</p><p>2</p>");

      [Fact]
      public void Remove_empty_spaces() =>
          Publish("<p> Hello </p>")
          .Should()
          .Be("<p>Hello</p>");

      [Fact]
      public void Wrap_codes() =>
           Publish("<pre class=\"code\"><b>CODE</b></pre>")
          .Should()
          .Be("<div class=\"code\"><pre><b>CODE</b></pre></div>");

      [Fact]
      public void Wrap_terminals() =>
          Publish("<pre class=\"terminal\"><b>CMD</b></pre>")
          .Should()
          .Be("<div class=\"cmd\"><pre><b>CMD</b></pre></div>");

      [Fact]
      public void Wrap_notes() =>
          Publish("<div class=\"note\"><b>NOTE</b></div>")
          .Should()
          .Be("<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>");

      [Fact]
      public void Wrap_warnings() =>
          Publish("<div class=\"warning\"><b>WARN</b></div>")
          .Should()
          .Be("<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>");

      [Fact]
      public void Unordered_lists() =>
          Publish("<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>")
          .Should()
          .Be("<ul><li>I1</li><li><b>I2</b></li></ul>");

      [Fact]
      public void Ordered_lists() =>
          Publish("<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>")
          .Should()
          .Be("<ol><li>I1</li><li><b>I2</b></li></ol>");

      [Fact]
      public void Dont_touch_insiders() =>
          Publish("<p contenteditable=\"true\"><strong>Hello</strong>World</p>")
          .Should()
          .Be("<p><strong>Hello</strong>World</p>");

      [Fact]
      public void Set_img_src_to_file() =>
          Publish("<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"><figcaption contenteditable=\"true\">CAP</figcaption></figure>")
          .Should()
          .Be("<figure><img src=\"\\images\\posts\\the-post\\pic.png\" alt=\"CAP\"><figcaption>CAP</figcaption></figure>");

      [Fact]
      public void Figures_without_captions() =>
          Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>")
          .Should()
          .Be("<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>");

      [Fact]
      public void Figures_with_empty_captions() =>
          Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"><figcaption></figcaption></figure>")
          .Should()
          .Be("<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>");

      #endregion

      [Fact]
      public void Throw_when_filename_is_not_available()
      {
         Action act = () => RenderImages("<figure><img src=\"data:image/jpeg;base64,DATA\"></figure>");
         act.Should().Throw<InvalidOperationException>();
      }

      [Fact]
      public void Multiple_images_with_the_same_name()
      {
         var draft = new Draft();
         draft.Info = new PostInfo { Title = "the post" };
         draft.Content = "<figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure><figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>";
         var images = draft.RenderImages();
         var publish = draft.Publish();

         publish.PostContent
             .Content
             .Should()
             .Be("<figure><img src=\"\\images\\posts\\the-post\\pic.png\"></figure><figure><img src=\"\\images\\posts\\the-post\\pic-1.png\"></figure>");
         images.First()
             .Filename
             .Should()
             .Be("pic.png");
         images.ElementAt(1)
             .Filename
             .Should()
             .Be("pic-1.png");
      }

      [Fact]
      public void Update_content_to_file_paths_instead_of_data_urls()
      {
         var draft = new Draft();
         draft.Info = new PostInfo { Title = "the post" };
         draft.Content = "<figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>";

         draft.RenderImages();

         draft.Content
             .Should()
             .Be("<figure><img src=\"\\images\\posts\\the-post\\pic.png\"></figure>");
      }

      [Fact]
      public void Update_img_srcs_when_title_changes()
      {
         var draft = new Draft();
         draft.Info = new PostInfo { Title = "the post" };
         draft.Content = "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"></figure>";

         draft.Info.Title = "new title";
         draft.RenderImages();

         draft.Content
             .Should()
             .Be("<figure><img src=\"\\images\\posts\\new-title\\pic.png\"></figure>");
      }
   }
}
