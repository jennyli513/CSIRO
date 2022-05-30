using System.Threading.Tasks;
using CSIRO.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ArchiDemo.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> userManager { get; }
        private SignInManager<IdentityUser> signInManager { get; }
        public AccountController(UserManager<IdentityUser> _userManager, SignInManager<IdentityUser> _signInManager)
        {
            this.userManager = _userManager;
            this.signInManager = _signInManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel m)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = m.Email, Email = m.Email };
                var result = await userManager.CreateAsync(user, m.Password);

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
                    var confirmationLink = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, token = token }, Request.Scheme);
                    //send local confLink
                    await System.IO.File.WriteAllTextAsync("confLink.txt", confirmationLink.ToString());
                    ViewBag.ErrorTitle = "Registration success";
                    ViewBag.ErrorMessage = "Before you login, please click the link provided to your email:\n" + m.Email;
                    return View("Error");
                }
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
            }
            return View(m);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (null == userId || null == token) RedirectToAction("Register");
            var user = await userManager.FindByIdAsync(userId);
            ViewBag.ErrorTitle = "";
            if (null == user)
            {
                ViewBag.ErrorMessage = userId + " is null";
                return View("Error");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.ErrorMessage = "Registration success";
                return View("Login");
            }
            ViewBag.ErrorMessage = "Email could not be confirmed";
            return View("Login");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel m)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(m.Email, m.Password, m.RememberMe, false);
                if (result.Succeeded)
                {
                    var user = await userManager.FindByNameAsync(m.Email);
                    var userId = user.Id;
                    HttpContext.Session.SetString("userId", userId);
                    return RedirectToAction("ShowCandidates", "Candidate");
                }
                ModelState.AddModelError("", "Invalid attempt");
            }
            return View(m);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
