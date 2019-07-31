using Blog.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Blog.Controllers
{
   public class ExperienceViewModel
   {
       public int  Id {get;set;}
       public string     Company {get;set;}
       public string     Position {get;set;}
       public string     Content {get;set;}
       public string     StartDate {get;set;}
       public string     EndDate {get;set;}
   }

   public class DeveloperViewModel
   {
      public int Id{get;set;}
      public string Summary{get;set;}
      public List<ExperienceViewModel> Experiences{get;set;}
   }

   [ApiController]
   public class TestController : Controller
   {
      [HttpGet("/api/developer")]
      public ActionResult<DeveloperViewModel> Get()
      {
         return new DeveloperViewModel
         {
            Id = 1,
            Summary = "<p contenteditable=\"true\">Some Content</p>",
            Experiences = new List<ExperienceViewModel>
            {
               new ExperienceViewModel
               {
                  Id = 1,
                  Company = "Parmis",
                  Position = "Senior Developer",
                  Content = "<p contenteditable='true'> parmis senior developer </p>",
                  StartDate = "2017-01-29",
                  EndDate = "2019-01-29"
               }, 
               new ExperienceViewModel
               {
                  Id = 2,
                  Company = "Freelancer",
                  Position = "C# Desktop Developer",
                  Content = "<p contenteditable='true'> freelance desktop developer </p>",
                  StartDate = "2016-03-25",
                  EndDate = "2017-03-25"
               }
            }
         };
      }
   }
}
