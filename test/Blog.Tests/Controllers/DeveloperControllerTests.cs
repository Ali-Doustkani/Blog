using Blog.Controllers;
using Blog.Services.DeveloperSaveCommand;
using Blog.Services.DeveloperSaveQuery;
using FluentAssertions;
using FluentAssertions.Json;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.Controllers
{
   public class DeveloperControllerTests  
   {
      public DeveloperControllerTests( )
      {
         _mediator = Substitute.For<IMediator>();
         _controller = new DeveloperController(_mediator);
      }

      private readonly DeveloperController _controller;
      private readonly IMediator _mediator;

      [Fact]
      public async Task Get_returns_204_when_no_developer_is_available()
      {
         var result = await _controller.Get() as StatusCodeResult;
         result.Should().BeOfType<NoContentResult>();
      }

      [Fact]
      public async Task Get_returns_200_when_a_developer_exists()
      {
         _mediator.Send(Arg.Any<DeveloperSaveQuery>())
            .Returns(Task.FromResult(new DeveloperSaveCommand()));
         var result = await _controller.Get();
         result.Should().BeOfType<OkObjectResult>();
      }

      [Fact]
      public async Task Put_returns_201_when_new_developer_is_added()
      {
         _mediator.Send(Arg.Any<DeveloperSaveCommand>())
            .Returns(Task.FromResult(DeveloperSaveResult.MakeSuccess(true, new[] { 1 }, new[] { 2 }, new[] { 3 })));
         var result = await _controller.Put(null) as CreatedAtActionResult;
         result.Value.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 1 },
            SideProjects = new[] { 2 },
            Educations = new[] { 3 }
         });
      }

      [Fact]
      public async Task Put_returns_200_when_developer_is_updated()
      {
         _mediator.Send(Arg.Any<DeveloperSaveCommand>())
            .Returns(Task.FromResult(DeveloperSaveResult.MakeSuccess(false, new[] { 1 }, new[] { 2 }, new[] { 3 })));
         var result = await _controller.Put(null) as OkObjectResult;
         result.Value.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 1 },
            SideProjects = new[] { 2 },
            Educations = new[] { 3 }
         });
       }
   }
}
