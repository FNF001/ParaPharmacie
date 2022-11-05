using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParaPharmacie.Data;
using ParaPharmacie.Models;
using ParaPharmacie.ViewModel;

namespace ParaPharmacie.Controllers
{
    public class AccountsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EcommerceContext _context;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.categories = GetGategories();
            base.OnActionExecuting(filterContext);
        }

        public AccountsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, EcommerceContext context)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
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

        public SelectList GetGategories()
        {
            SelectList Categories = new SelectList(_context.Categories, "CatId", "CatName");
            ViewBag.categories = Categories;
            return Categories;
        }

    }
}
