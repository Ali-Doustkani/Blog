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
         if (result.Failed)
            return BadRequest(result);

         _context.Update(developer);
         _context.SaveChanges();

         if (created)
            return CreatedAtAction(nameof(Get), result);

         return Ok(result);
      }

      private bool CreateOrGet(DeveloperUpdateCommand updateCommand, out Developer developer)
      {
         if (_context.Developers.Any())
         {
            developer = _context.GetDeveloper();
            return true;
         }

         developer = new Developer(updateCommand.Summary, updateCommand.Skills);
         return false;
      }
   }
}
