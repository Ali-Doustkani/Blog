using Blog.Services.DraftDeleteCommand;
using Blog.Services.DraftListQuery;
using Blog.Services.DraftQuery;
using Blog.Services.DraftSaveCommand;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
   [ApiController]
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
   [Route("/api/drafts")]
   public class DraftController : ControllerBase
   {
      public DraftController(IMediator mediator)
      {
         _mediator = mediator;
      }

      private readonly IMediator _mediator;

      [HttpGet]
      public async Task<IActionResult> Get() =>
         Ok(await _mediator.Send(new DraftListQuery()));

      [HttpGet("{id}")]
      public async Task<IActionResult> GetById(int id) =>
         Ok(await _mediator.Send(new DraftQuery { Id = id }));

      [HttpPatch("{id}")]
      [HttpPost]
      public async Task<IActionResult> Save(DraftSaveCommand draft)
      {
         var result = await _mediator.Send(draft);
         if (result.Failed)
            return BadRequest(result.Errors);
         return Ok(result.Id);
      }

      [HttpDelete("{id}")]
      public async Task<IActionResult> Delete(int id)
      {
         await _mediator.Send(new DraftDeleteCommand { Id = id });
         return Ok();
      }
   }
}
