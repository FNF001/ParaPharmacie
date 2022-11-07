using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared.CodeModifier.CodeChange;
using Microsoft.EntityFrameworkCore;
using ParaPharmacie.Data;
using ParaPharmacie.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Emit;
using System.Xml.Linq;

namespace ParaPharmacie.Controllers
{
    [Route("Checkout/[action]")]
    public class CheckoutsController : Controller
    {
        private readonly EcommerceContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.categories = GetGategories();
            base.OnActionExecuting(filterContext);
        }

        public CheckoutsController(EcommerceContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            decimal TotalCartPrice = 0;
            int TotalProd = 0;
            int TotalQty = 0;
            var user = await _userManager.GetUserAsync(User);
            var CartItemsAllDefined = _context.ShoppingCarts.Include(p => p.Product).Where(u => u.UserId == user.Id).ToList();
            var CartItems = GetShoppingCartsByUser(CartItemsAllDefined, user);
            foreach (var item in CartItems)
            {
                TotalCartPrice += item.Product.Price * item.Qty;
                TotalProd++;
                TotalQty = TotalQty + item.Qty;
                ViewBag.TotalQty = TotalQty;
                TotalQty = TotalQty + item.Qty;
            }
            ViewBag.CartItems = CartItems;
            ViewBag.TotalCartPrice = TotalCartPrice;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddressAndPayment(Order model)
        {
            decimal TotalCartPrice = 0;
            int TotalProd = 0;
            int TotalQty = 0;
            var user = await _userManager.GetUserAsync(User);
            model.UserId = user.Id;
            model.ApplicationUser = user;
            var CartItemsAllDefined = _context.ShoppingCarts.Include(p => p.Product).Where(u => u.UserId == user.Id).ToList();
            var CartItems = GetShoppingCartsByUser(CartItemsAllDefined, user);
            model.OrderItems = CartItems;
            model.OrderDate = DateTime.Now;
            foreach (var item in CartItems)
            {
                TotalCartPrice += item.Product.Price * item.Qty;
                TotalProd++;
                TotalQty = TotalQty + item.Qty;
                ViewBag.TotalQty = TotalQty;
                TotalQty = TotalQty + item.Qty;
            }
            ViewBag.CartItems = CartItems;
            ViewBag.TotalCartPrice = TotalCartPrice;


            //if (ModelState.IsValid)
            //{

                var order = new Order
                {
                    OrderDate = DateTime.Now,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    AddressLine1 = model.AddressLine1,
                    AddressLine2 = model.AddressLine2,
                    Country = model.Country,
                    City = model.City,
                    State = model.State,
                    ZIPCode = model.ZIPCode,
                    Total = model.Total,
                    OrderItems = CartItems,
                    UserId = user.Id
                };

                // Save Order

                _context.Orders.Add(order);
                _context.SaveChanges();

                // Save Order Detail

                AddOrderDetail(order);

                // Need to Process the order Later

                return RedirectToAction("Complete",
                    new { id = order.OrderId });

            //}
            //return View("Checkout",model);

        }

        public void AddOrderDetail (Order order)
        {
            foreach (var item in order.OrderItems)
            { 
            var orderDetail = new OrderDetail
            {
                OrderId = order.OrderId,
                CartId = item.CartId,
                Qty = item.Qty,
                OrderDate = order.OrderDate,
                ProId = item.ProId,
                ProName = item.Product.ProName,
                UserId = order.UserId,
                UserName = item.ApplicationUser.UserName,
                Name = item.ApplicationUser.Name,
                Status = "New"
            };
                _context.OrdersDetails.Add(orderDetail);
                _context.SaveChanges();
            }
        }

        public SelectList GetGategories()
        {
            SelectList Categories = new SelectList(_context.Categories, "CatId", "CatName");
            ViewBag.categories = Categories;
            return Categories;
        }

        public IActionResult Complete(int id)
        {
            return View(id);
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
