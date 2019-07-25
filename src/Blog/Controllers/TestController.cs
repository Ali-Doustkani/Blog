using Blog.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Blog.Controllers
{
   [ApiController]
   public class TestController : Controller
   {
      [HttpGet("/api/developer")]
      public ActionResult<Developer> Get()
      {
         var dev = new Developer
         {
            Id = 1,
            Summary = "<p contenteditable=\"true\">Some Content</p>",
         };
         dev.Experiences.Add(new WorkExperience
         {
            Id = 1,
            Company = "Parmis",
            Position = "Senior Developer",
            Content = "<p contenteditable='true'> parmis senior developer </p>",
            StartDate = new DateTime(2017, 1, 1),
            EndDate = new DateTime(2019, 1, 1)
         });
         dev.Experiences.Add(new WorkExperience
         {
            Id = 2,
            Company = "Freelancer",
            Position = "C# Desktop Developer",
            Content = "<p contenteditable='true'> freelance desktop developer </p>",
            StartDate = new DateTime(2016, 1, 1),
            EndDate = new DateTime(2017, 1, 1)
         });
         return dev;
      }
   }
}
