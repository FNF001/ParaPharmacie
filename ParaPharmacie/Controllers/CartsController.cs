using Microsoft.AspNetCore.Mvc;

namespace ParaPharmacie.Controllers
{
    public class CartsController : Controller
    {
        public IActionResult Cart()
        {
            return View();
        }
    }
}
