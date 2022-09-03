using Microsoft.AspNetCore.Mvc;
using ParaPharmacie.Models;

namespace ParaPharmacie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Create(Category model)
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult Edit()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Edit(Category model)
        {
            return View();
        }

        
        public IActionResult Delete()
        {
            return View();
        }

    }
}
