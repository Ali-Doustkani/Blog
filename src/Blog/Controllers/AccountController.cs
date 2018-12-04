using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    public class AccountController : Controller
    {
        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager;
        }

        private readonly SignInManager<IdentityUser> _signInManager;

        public IActionResult Login(string returnUrl = null)
        {
            ViewData["returnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                return RedirectToAction(nameof(HomeController.Index), Extensions.NameOf<HomeController>());
            }

            ModelState.AddModelError(string.Empty, "Wrong username or password");
            return View();
        }

        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index), Extensions.NameOf<HomeController>());
        }
    }
}
