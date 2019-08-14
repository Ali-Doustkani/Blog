using Blog.Validation;
using FluentAssertions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Xunit;

namespace Blog.Tests.Validation
{
   public class ValidationResponseBuilderTests
   {
      public class TheModel
      {
         [Required]
         public string Summary { get; set; }
      }

      public class NestedModel
      {
         public TheModel Prop { get; set; }
      }

      public class NullableModel
      {
         [Required]
         public int? Value { get; set; }
      }

      public class EmptyCollection
      {
         [Required]
         public IEnumerable<string> Values { get; set; }
      }

      public ValidationResponseBuilderTests()
      {
         builder = new ValidationResponseBuilder();
      }

      private readonly ValidationResponseBuilder builder;

      [Theory]
      [InlineData(null)]
      [InlineData("")]
      [InlineData("  ")]
      public void Check_required_string_properties(string value)
      {
         var model = new TheModel { Summary = value };

         builder.BuildFrom(model);

         builder.Invalid.Should().BeTrue();
         builder.Result.Errors.Should().ContainEquivalentOf(new
         {
            Error = ValidationErrorType.IsRequired,
            Path = new[] { "summary" }
         });
      }

      [Fact]
      public void Check_required_nullable_properties()
      {
         builder.BuildFrom(new NullableModel { Value = null });

         builder.Invalid.Should().BeTrue();
         builder.Result.Errors.Should().ContainEquivalentOf(new
         {
            Error = ValidationErrorType.IsRequired,
            Path = new[] { "value" }
         });
      }

      [Fact]
      public void Approve_required_properties()
      {
         var model = new TheModel { Summary = "text" };

         builder.BuildFrom(model);

         builder.Invalid.Should().BeFalse();
         builder.Result.Should().BeNull();
      }

      [Fact]
      public void Check_nested_properties()
      {
         var model = new NestedModel
         {
            Prop = new TheModel()
         };

         builder.BuildFrom(model);

         builder.Invalid.Should().BeTrue();
         builder.Result.Errors.Should().ContainEquivalentOf(new
         {
            Error = ValidationErrorType.IsRequired,
            Path = new[] { "prop", "summary" }
         });
      }

      [Fact]
      public void Return_null_if_every_property_is_ok()
      {
         var model = new TheModel { Summary = "text" };

         builder.BuildFrom(model);

         builder.Invalid.Should().BeFalse();
         builder.Result.Should().BeNull();
      }

      [Fact]
      public void Check_empty_collections()
      {
         builder.BuildFrom(new EmptyCollection { Values = new string[] { } });

         builder.Invalid.Should().BeTrue();
         builder.Result.Errors.Should().BeEquivalentTo(new
         {
            Error = ValidationErrorType.IsEmpty,
            Path = new[] { "values" }
         });
      }
   }
}
