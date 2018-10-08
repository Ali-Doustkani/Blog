using Microsoft.AspNetCore.Mvc;

namespace src.Controllers
{
    public class HomeController: Controller
    {
        public ViewResult Index()
        {
            return View("About");
        }
    }
}
