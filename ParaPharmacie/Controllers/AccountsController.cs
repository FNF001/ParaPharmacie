using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ParaPharmacie.Models;
using ParaPharmacie.ViewModel;

namespace ParaPharmacie.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public IActionResult Login()
        {
            ViewBag.activepage = "Login";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
           ViewBag.activepage = "Login";
           if (ModelState.IsValid)
           {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, true, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(model);
                }
           }

            return View(model);
        }
        public IActionResult Register()
        {
            ViewBag.activepage = "Register";
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register (RegisterVM model)
        {
            ViewBag.activepage = "Register";
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Id = model.Id,
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                };
                user.Id = Guid.NewGuid().ToString();
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return View(model);
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
