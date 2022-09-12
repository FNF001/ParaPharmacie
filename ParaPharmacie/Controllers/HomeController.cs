using Microsoft.AspNetCore.Mvc;
using ParaPharmacie.Areas.Admin.Controllers;
using ParaPharmacie.Data;
using ParaPharmacie.Models;
using ParaPharmacie.ViewModel;
using System.Diagnostics;

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

        public IActionResult Index()
        {
            var model = new IndexVM{
                Categories = _context.Categories.ToList(),
                Products = _context.Products.Take(10).ToList()
            };
            return View(model);
        }

        public IActionResult Product()
        {
            var products = _context.Products.ToList();
            return View(products);
        }

        public IActionResult ProductDetailes()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Contact (Contact model)
        {
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