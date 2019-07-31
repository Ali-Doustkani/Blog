using Blog.Services.Administrator;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
   [ApiController]
   [Route("/api")]
   public class AdministratorApi : Controller
   {
      public AdministratorApi(Service service)
      {
         _service = service;
      }

      private readonly Service _service;

      [HttpGet("developer")]
      public ActionResult<Developer> GetDeveloper()
      {
         return _service.GetDeveloper();
      }
   }
}
