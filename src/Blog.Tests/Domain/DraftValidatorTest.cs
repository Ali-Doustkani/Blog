using Blog.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class DraftValidatorTest
   {
      public DraftValidatorTest()
      {
         _context = new BlogContext(Db.CreateOptions());
         _context.Database.EnsureCreated();
         _validator = new DraftValidator(_context);
      }

      private readonly DraftValidator _validator;
      private readonly BlogContext _context;

      [Fact]
      public void When_post_is_farsi_englishUrl_must_be_set()
      {
         var draft = new Draft
         {
            Info = new PostInfo { Language = Language.Farsi },
            Content = "content"
         };

         _validator
            .Validate(draft)
            .Should()
            .ContainEquivalentOf(new
            {
               Property = "EnglishUrl",
               Message = "EnglishUrl is required for Farsi posts"
            });
      }

      [Fact]
      public void Title_must_be_unique()
      {
         _context.Drafts.Add(new Draft
         {
            Id = 22,
            Info = new PostInfo { Id = 22, Title = "T1", Summary = "summary", Tags = "tags" },
            Content = "content"
         });
         _context.SaveChanges();
         var draft = new Draft
         {
            Info = new PostInfo { Title = "T1" },
            Content = "content"
         };

         _validator
            .Validate(draft)
            .Should()
            .ContainEquivalentOf(new
            {
               Property = "Title",
               Message = "This title already exists in the database"
            });
      }

      [Fact]
      public void Error_when_language_of_code_block_is_not_specified()
      {
         var draft = new Draft { Info = new PostInfo() };
         draft.Content = "<pre class=\"code\">some code</pre>";

         _validator
            .Validate(draft)
            .Should()
            .ContainEquivalentOf(new
            {
               Property = "Content",
               Message = "Language is not specified for the code block #1"
            });
      }

      [Fact]
      public void Dont_error_for_empty_code_blocks()
      {
         var draft = new Draft { Info = new PostInfo() };
         draft.Content = "<pre class=\"code\"> </pre>";

         _validator
            .Validate(draft)
            .Should()
            .BeEmpty();
      }

      [Fact]
      public void Error_when_invalid_language_is_set()
      {
         var draft = new Draft { Info = new PostInfo() };
         draft.Content = string.Join(Environment.NewLine, "<pre class=\"code\">", "clojure", "some code</pre>");

         _validator
            .Validate(draft)
            .Should()
            .ContainEquivalentOf(new
            {
               Property = "Content",
               Message = "Specified language in code block #1 is not valid (clojure...)"
            });
      }
   }
}
