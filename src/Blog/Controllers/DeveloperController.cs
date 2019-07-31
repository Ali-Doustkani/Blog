using Blog.Services.DeveloperStory;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
   [ApiController]
   [Route("/api/developer")]
   public class DeveloperController : Controller
   {
      public DeveloperController(IDeveloperService service)
      {
         _service = service;
      }

      private readonly IDeveloperService _service;

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
         var result = _service.Save(developer);
         if (result.Status == Status.Created)
            return CreatedAtAction(nameof(Get), new { result.Experiences, result.SideProjects });

         return Ok(new { result.Experiences, result.SideProjects });
      }
   }
}
