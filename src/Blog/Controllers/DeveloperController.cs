using AutoMapper;
using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blog.Controllers
{
   [ApiController]
   [Route("/api/developer")]
   public class DeveloperController : ControllerBase
   {
      public DeveloperController(IMapper mapper, BlogContext context, IStorageState storageState)
      {
         _mapper = mapper;
         _context = context;
         _storageState = storageState;
      }

      private readonly IMapper _mapper;
      private readonly BlogContext _context;
      private readonly IStorageState _storageState;

      [HttpGet]
      public ActionResult<DeveloperUpdateCommand> Get()
      {
         var result = _mapper.Map<DeveloperUpdateCommand>(_context.GetDeveloper());
         if (result == null)
            return NoContent();
         return result;
      }

      [HttpPut]
      public IActionResult Put(DeveloperUpdateCommand updateCommand)
      {
         var created = CreateOrGet(updateCommand, out Developer developer);

         var result = developer.Update(updateCommand, _storageState);

         _context.Update(developer);
         _context.SaveChanges();

         if (created)
            return CreatedAtAction(nameof(Get), new { result.Experiences, result.SideProjects, result.Educations });

         return Ok(new { result.Experiences, result.SideProjects, result.Educations });
      }

      private bool CreateOrGet(DeveloperUpdateCommand updateCommand, out Developer developer)
      {
         if (_context.Developers.Any())
         {
            developer = _context.GetDeveloper();
            return false;
         }

         developer = new Developer(updateCommand.Summary, updateCommand.Skills);
         return true;
      }
   }
}
