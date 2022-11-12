using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
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

        public UsersController(EcommerceContext context)
        {
            _context = context;
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
                                      RoleNames = (from userRole in _context.Roles
                                                   join role in _context.Roles on userRole.Id
                                                       equals role.Id
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
    }
}
