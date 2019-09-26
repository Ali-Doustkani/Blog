using Blog.Services.DeveloperSaveCommand;
using Blog.Services.DeveloperSaveQuery;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
   [ApiController]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   [Route("/api/developer")]
   public class DeveloperController : ControllerBase
   {
      public DeveloperController(IMediator mediator)
      {
         _mediator = mediator;
      }

      private readonly IMediator _mediator;

      [HttpGet]
      public async Task<IActionResult> Get()
      {
         var result = await _mediator.Send(new DeveloperSaveQuery());
         if (result == null)
            return NoContent();
         return Ok(result);
      }

      [HttpPut]
      public async Task<IActionResult> Put(DeveloperSaveCommand command)
      {
         var result = await _mediator.Send(command);

         if (result.Created)
            return CreatedAtAction(nameof(Get), new { result.Experiences, result.SideProjects, result.Educations });

         return Ok(new { result.Experiences, result.SideProjects, result.Educations });
      }
   }
}
