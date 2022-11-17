using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NuGet.Versioning;
using ParaPharmacie.Data;
using ParaPharmacie.Models;
using ParaPharmacie.ViewModel;
using System.Data;
using System.Drawing.Printing;

namespace ParaPharmacie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public UsersController(EcommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<List<UsersWithRolesVM>> GetPage(IQueryable<UsersWithRolesVM> result, int pagenumber, int pagesize)
        {
            int Pagesize = pagesize;
            decimal rowCount = await _context.Users.CountAsync();
            var pagecount = Math.Ceiling(rowCount / Pagesize);
            if (pagenumber > pagecount)
            {
                pagenumber = 1;
            }
            pagenumber = pagenumber <= 0 ? 1 : pagenumber;
            int skipCount = (pagenumber - 1) * Pagesize;
            var pageData = await result
                .Skip(skipCount)
                .Take(Pagesize)
                .ToListAsync();
            ViewBag.CurrentPage = pagenumber;
            ViewBag.PageCount = pagecount;

            return pageData;

        }
        public async Task<IActionResult> Users(int page, int pagesize)
        {
            if (pagesize == 0)
            {
                pagesize = 3;
            }
            if (pagesize != 3)
            {
                ViewBag.pagesize = pagesize;
            }
            var usersWithRoles = (from user in _context.Users
                                  select new
                                  {
                                      Id = user.Id,
                                      Name = user.Name,
                                      UserName = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in _context.UserRoles
                                                   join role in _context.Roles on userRole.RoleId
                                                       equals role.Id where userRole.UserId == user.Id
                                                   select role.Name).FirstOrDefault()
                                  }).Select(p => new UsersWithRolesVM()
            {
            Id = p.Id,
            Name = p.Name,
            UserName = p.UserName,
            Email = p.Email,
            RoleName = p.RoleNames
            });
            var model = await GetPage(usersWithRoles, page, pagesize);
            return View(model);
            
        }

        public IActionResult ChangeUserRole(string? id)
        {
            var userWithRole = (from user in _context.Users
                                  select new
                                  {
                                      Id = user.Id,
                                      Name = user.Name,
                                      UserName = user.UserName,
                                      Email = user.Email,
                                      RoleNames = (from userRole in _context.UserRoles
                                                   join role in _context.Roles on userRole.RoleId
                                                       equals role.Id where userRole.UserId == user.Id
                                                   select role.Name).FirstOrDefault()
                                  }).Where(p=>p.Id == id).Select(p => new UsersWithRolesVM()
                                  {
                                      Id = p.Id,
                                      Name = p.Name,
                                      UserName = p.UserName,
                                      Email = p.Email,
                                      RoleName = p.RoleNames
                                  }).FirstOrDefault();


            ViewData["RoleList"] = new SelectList(_context.Roles, "Id", "Name");
            return View((UsersWithRolesVM)userWithRole);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeUserRole(string? id, UsersWithRolesVM model)
        {
            
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            var Role = _context.Roles.Find(model.RoleName).Name.ToString();
            List<string> roles = _context.Roles.Select(r => r.Name).ToList();
            var result1 = await _userManager.RemoveFromRolesAsync(user,roles);
            var result2 = await _userManager.AddToRoleAsync(user, Role);
            return RedirectToAction("Users");
        }
    }
}
