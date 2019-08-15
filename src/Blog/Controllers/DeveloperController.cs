using Blog.Services.DeveloperStory;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
   [ApiController]
   [Route("/api/developer")]
   public class DeveloperController : ControllerBase
   {
      public DeveloperController(IDeveloperServices service)
      {
         _service = service;
      }

      private readonly IDeveloperServices _service;

      [HttpGet]
      public ActionResult<DeveloperEntry> Get()
      {
         var result = _service.Get();
         if (result == null)
            return NoContent();
         return result;
      }

      [HttpPut]
      public IActionResult Put(DeveloperEntry developer)
      {
         if (developer == null)
            return Ok();
         var result = _service.Save(developer);
         if (result.Status == Status.Created)
            return CreatedAtAction(nameof(Get), null);

         return Ok();
      }
   }
}
