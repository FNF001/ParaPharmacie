using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParaPharmacie.Data;
using ParaPharmacie.Models;

namespace ParaPharmacie.Controllers
{

    [Route("Carts/[action]")]
    public class CartsController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public int _TotalQty ;

        public CartsController(EcommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User);
            var result = _context.ShoppingCarts.Include(p => p.Product).Where(u => u.UserId == user.Id).ToList();
            return View(result);
        }
        
        [HttpPost]
        public async Task<IActionResult> AddToCart(ShoppingCart model, int qty)
        {
            var product = _context.Products.FirstOrDefault(p=>p.ProId == model.ProId);
            
            var user = await _userManager.GetUserAsync(User);

            var cart = new ShoppingCart
            {
                UserId = user.Id,
                ProId = product.ProId,
                Qty = qty
            };
            var shopcart = _context.ShoppingCarts.FirstOrDefault(u => u.UserId == user.Id && u.ProId == model.ProId);

            if (qty <= 0)
            {
                qty = 1;
            }
            if(shopcart == null)
              _context.ShoppingCarts.Add(cart);
            else
                shopcart.Qty += model.Qty;

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int id)
        {
            
            var user = await _userManager.GetUserAsync(User);

            var shopcart = _context.ShoppingCarts.FirstOrDefault(u => u.UserId == user.Id && u.CartId == id);

            if (shopcart != null)
            {
                _context.ShoppingCarts.Remove(shopcart);
            }
            _context.SaveChanges();

            return RedirectToAction(nameof(Cart));
        }
    
        public int IncreaseQtyItem(int qty)
        {
            qty = qty + 1;
            ViewBag.CurrentQty = qty;
            return qty;
        }

        public int DecreaseQtyItem(int qty)
        {
            qty = qty - 1;
            ViewBag.CurrentQty = qty;
            return qty;
        }

        public async Task<int> GetTotalQty()
        {
            int TotalQty = 0;
            var user = await _userManager.GetUserAsync(User);
            var result = _context.ShoppingCarts.Include(p => p.Product).Where(u => u.UserId == user.Id).ToList();
            foreach (var item in result)
            {
                TotalQty = TotalQty + item.Qty;
            }
            return TotalQty;
        }
    }
}
