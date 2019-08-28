using Blog.CQ.DeveloperSaveCommand;
using Blog.CQ.DeveloperSaveQuery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
   [ApiController]
   [Route("/api/developer")]
   public class DeveloperController : ControllerBase
   {
      public DeveloperController(IMediator mediator)
      {
         _mediator = mediator;
      }

      private readonly IMediator _mediator;

      [HttpGet]
      public async Task<ActionResult<DeveloperSaveCommand>> Get()
      {
         var result = await _mediator.Send(new DeveloperSaveQuery());
         if (result == null)
            return NoContent();
         return result;
      }

      [HttpPut]
      public async Task<IActionResult> Put(DeveloperSaveCommand command)
      {
         var result = await _mediator.Send(command);

         if (result.Created)
            return CreatedAtAction(nameof(Get), new { result.UpdateResult.Experiences, result.UpdateResult.SideProjects, result.UpdateResult.Educations });

         return Ok(new { result.UpdateResult.Experiences, result.UpdateResult.SideProjects, result.UpdateResult.Educations });
      }
   }
}
