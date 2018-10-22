using Blog.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Route("admin")]
    public class AdministratorController : Controller
    {
        [Route("post")]
        public ViewResult Post()
        {
            var newPost = new Post
            {
                PublishDate = DateTime.Now
            };
            return View(newPost);
        }
    }
}
