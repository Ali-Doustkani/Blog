using Blog.Utils;
using Blog.ViewModels.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Blog.Controllers
{
   public class AccountController : Controller
   {
      public AccountController(SignInManager<IdentityUser> signInManager, ILogger<AccountController> logger)
      {
         _signInManager = signInManager;
         _logger = logger;
      }

      private readonly SignInManager<IdentityUser> _signInManager;
      private readonly ILogger _logger;

      [IgnoreMigration]
      public IActionResult Login(string returnUrl = null)
      {
         ViewData["returnUrl"] = returnUrl;
         return View();
      }

      [HttpPost]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Login(LoginEntry vm, string returnUrl = null)
      {
         ViewData["returnUrl"] = returnUrl;

         if (!ModelState.IsValid)
            return View();

         var result = await _signInManager.PasswordSignInAsync(vm.Username, vm.Password, true, true);
         if (result.Succeeded)
         {
            _logger.LogInformation("Login Succeeded");

            if (returnUrl != null)
               return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
         }

         ModelState.AddModelError(string.Empty, "Wrong username or password");

         _logger.LogInformation("Login Failed with {0} error(s)", ModelState.ErrorCount);

         return View();
      }

      [Authorize]
      [ValidateAntiForgeryToken]
      public async Task<IActionResult> Logout()
      {
         await _signInManager.SignOutAsync();
         return RedirectToAction("Index", "Home");
      }
   }
}
