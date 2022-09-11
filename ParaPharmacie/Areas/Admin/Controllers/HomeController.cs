using Microsoft.AspNetCore.Mvc;
using ParaPharmacie.Data;

namespace ParaPharmacie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly EcommerceContext _context;

        public HomeController(EcommerceContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult ShowMessage()
        {
            var message = _context.Contacts.ToList();
            return View(message);
        }


    }
}
