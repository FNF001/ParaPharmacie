using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.categories = GetGategories();
            base.OnActionExecuting(filterContext);
        }

        public CartsController(EcommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Cart()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var CartItemsAllDefined = _context.ShoppingCarts.Include(p => p.Product).Where(u => u.UserId == user.Id).ToList();
                var CartItems = GetShoppingCartsByUser(CartItemsAllDefined, user);
                return View(CartItems);
            }
            catch
            {
                return RedirectToAction("Login","Accounts");
            }

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
            
            // Check if shopping cart submitted in OrderDetails or not
            var CartIDs_ordersDetails = _context.OrdersDetails.Select(x => x.CartId).ToList();
            bool Submitted = false;
            if (shopcart != null)
            {
                foreach (var item in CartIDs_ordersDetails)
                {
                    if (shopcart.CartId == item) Submitted = true;
                }
            }

            if (qty <= 0)
            {
                qty = 1;
            }
            if(shopcart == null)
              _context.ShoppingCarts.Add(cart);
            else if (Submitted == false)
               shopcart.Qty += model.Qty;
            else if (Submitted = true)
              _context.ShoppingCarts.Add(cart);

            _context.SaveChanges();

            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public async Task<IActionResult> RemoveItem(int id)
        {
            
            var user = await _userManager.GetUserAsync(User);

            var shopcart = _context.ShoppingCarts.FirstOrDefault(u => u.UserId == user.Id && u.CartId == id );

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
            var CartItemsAllDefined = _context.ShoppingCarts.Include(p => p.Product).Where(u => u.UserId == user.Id).ToList();
            var CartItems = GetShoppingCartsByUser(CartItemsAllDefined, user);
            foreach (var item in CartItems)
            {
                TotalQty = TotalQty + item.Qty;
            }
            return TotalQty;
        }

        public SelectList GetGategories()
        {
            SelectList Categories = new SelectList(_context.Categories, "CatId", "CatName");
            ViewBag.categories = Categories;
            return Categories;
        }

        public List<ShoppingCart> GetShoppingCartsByUser(List<ShoppingCart> CartItems, ApplicationUser user)
        {
            var ShoppingCartsByUser = CartItems;
            var CartIDs_ordersDetails = _context.OrdersDetails.Select(x => x.CartId).ToList();

            foreach (var item in CartIDs_ordersDetails)
            {
                ShoppingCartsByUser.Remove(_context.ShoppingCarts.Include(p => p.Product).Where(u => u.UserId == user.Id && u.CartId == item).FirstOrDefault());
            }
            return ShoppingCartsByUser;
        }

    }
}
