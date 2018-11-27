using Blog.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Route("account")]
    public class AccountController : Controller
    {
        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        private readonly SignInManager<IdentityUser> _signInManager;

        [Route("login")]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("login")]
        public async Task<IActionResult> Login(LoginViewModel vm, string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View();

            var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, true);
            if (result.Succeeded)
            {
                if (returnUrl != null)
                    return Redirect(returnUrl);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Wrong username or password");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
