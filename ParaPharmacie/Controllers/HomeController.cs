using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ParaPharmacie.Areas.Admin.Controllers;
using ParaPharmacie.Data;
using ParaPharmacie.Models;
using ParaPharmacie.ViewModel;
using System.Diagnostics;
using System.Drawing.Printing;

namespace ParaPharmacie.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EcommerceContext _context;

        public HomeController(ILogger<HomeController> logger, EcommerceContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<List<Product>> GetPage (IQueryable<Product> result, int pagenumber,int pagesize)
        {
            int Pagesize = pagesize;
            decimal rowCount = await _context.Products.CountAsync();
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


        public IActionResult Index()
        {
            ViewBag.activepage = "Index"; 
            var model = new IndexVM{
                Categories = _context.Categories.ToList(),
                Products = _context.Products.Take(10).ToList()
            };
            return View(model);
        }

        public async Task<IActionResult> Product(int page, int pagesize)
        {
            ViewBag.activepage = "Product";
            if (pagesize == 0)
            {
                pagesize = 5;
            }
            if (pagesize != 5)
            {
                ViewBag.pagesize = pagesize;
            }
            var products = _context.Products;
            var model = await GetPage(products, page, pagesize);
            return View(model);
        }

        public async Task<IActionResult> ProductCategory(int id, int page, int pagesize)
        {
            ViewBag.activepage = "Product";
            if (id != 0)
            {
                ViewBag.id = id;
            }
            if (id == 0)
            {
                return RedirectToAction("Index");
            }
            if (pagesize == 0)
            {
                pagesize = 5;
            }
            if (pagesize != 5)
            {
                ViewBag.pagesize = pagesize;
            }
            var products = _context.Products.Where(c => c.CatId == id);
            var model = await GetPage(products, page, pagesize);
            return View(model);
        }

        public IActionResult SearchProduct(string NamePro)
        {
            ViewBag.activepage = "Product";
            var products = _context.Products.Where(p => p.ProName.Contains(NamePro)).ToList();
            return View(products);
        }

        public IActionResult ProductDetailes(int? id)
        {
            ViewBag.activepage = "ProductDetailes";
            var product = _context.Products.Include(x => x.Category)
                .FirstOrDefault(p => p.ProId == id);
            return View(product);
        }

        public IActionResult Contact()
        {
            ViewBag.activepage = "Contact";
            return View();
        }

        [HttpPost]
        public IActionResult Contact (Contact model)
        {
            ViewBag.activepage = "Contact";
            if (ModelState.IsValid)
            {
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}