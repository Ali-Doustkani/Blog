using Blog.Services.DeveloperStory;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
   [ApiController]
   [Route("/api/developer")]
   public class DeveloperController : Controller
   {
      public DeveloperController(Service service)
      {
         _service = service;
      }

      private readonly Service _service;

      [HttpGet]
      public ActionResult<DeveloperEntry> GetDeveloper()
      {
         var result = _service.GetDeveloper();
         if (result == null)
            return NoContent();
         return result;
      }
   }
}
