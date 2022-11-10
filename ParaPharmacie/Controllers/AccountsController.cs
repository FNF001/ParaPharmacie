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
    [Route("Accounts/[action]")]
    public class AccountsController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EcommerceContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.categories = GetGategories();
            base.OnActionExecuting(filterContext);
        }

        public AccountsController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, EcommerceContext context, RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _roleManager = roleManager;
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
           await CreateRolesandUsers();
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
            await CreateRolesandUsers();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    Id = model.Id,
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name
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
            await CreateRolesandUsers();
            return RedirectToAction("Index", "Home");
        }

        public SelectList GetGategories()
        {
            SelectList Categories = new SelectList(_context.Categories, "CatId", "CatName");
            ViewBag.categories = Categories;
            return Categories;
        }

        private async Task CreateRolesandUsers()
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid().ToString(),
                    UserName = "naderfarid.07@gmail.com",
                    Email = "naderfarid.07@gmail.com",
                    Name = "Nader Farid"
                };
                IdentityResult chkUser = await _userManager.CreateAsync(user, "FNF001001fnf#");

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // creating Creating Customer role     
            bool y = await _roleManager.RoleExistsAsync("Customer");
            if (!y)
            {
                var role = new IdentityRole();
                role.Name = "Customer";
                await _roleManager.CreateAsync(role);
            }
        }

    }
}
