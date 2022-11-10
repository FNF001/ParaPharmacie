using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ParaPharmacie.Data;
using ParaPharmacie.Models;
using ParaPharmacie.ViewModel;

namespace ParaPharmacie.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrdersController : Controller
    {
        private readonly EcommerceContext _context;

        public OrdersController(EcommerceContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDetail>> GetPageOrdersDetails(IQueryable<OrderDetail> result, int pagenumber, int pagesize,int id)
        {
            int Pagesize = pagesize;
            decimal rowCount = await _context.OrdersDetails.Where(o => o.OrderId == id).CountAsync();
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

        public async Task<List<Order>> GetPageOrders(IQueryable<Order> result, int pagenumber, int pagesize)
        {
            int Pagesize = pagesize;
            decimal rowCount = await _context.Orders.CountAsync();
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

        public async Task<IActionResult> OrdersDetails (int id, int page, int pagesize)
        {
            if (pagesize == 0)
            {
                pagesize = 3;
            }
            if (pagesize != 3)
            {
                ViewBag.pagesize = pagesize;
            }
            ViewBag.OrderId = id;
            var orderDetail = _context.OrdersDetails.Include(o=>o.Order).Include(u => u.ApplicationUser).Where(o=>o.OrderId == id);
            var model = await GetPageOrdersDetails(orderDetail, page, pagesize,id);
            return View(model);
        }

        public async Task<IActionResult> Orders(int page, int pagesize)
        {
            if (pagesize == 0)
            {
                pagesize = 3;
            }
            if (pagesize != 3)
            {
                ViewBag.pagesize = pagesize;
            }
            var order = _context.Orders.Include(u => u.ApplicationUser);
            var model = await GetPageOrders(order, page, pagesize);
            return View(model);
        }

        public IActionResult ChangeOrderStatus(int? id)
        {
            var orderDetail = _context.OrdersDetails.Find(id);
            SelectList statusList = new SelectList( new List<SelectListItem>
                {
                    new SelectListItem { Selected = false, Text = "New" , Value = "1"},
                    new SelectListItem { Selected = false, Text = "In Progress", Value = "2"},
                    new SelectListItem { Selected = false, Text = "Delivered", Value = "3"},
                }, "Text" , "Value", 1);
            ViewData["StatusList"] = new SelectList(statusList, "Text", "Value");
            return View(orderDetail);
        }

        [HttpPost]
        public async Task<IActionResult> ChangeOrderStatus(int id,int? orderId, OrderDetail model )
        {
            string? ChangedStatusConverted = GetStatusByValue(model.Status);
            var orderDetail = _context.OrdersDetails.Find(id);
            if (orderDetail != null)
            {
                orderDetail.Status = ChangedStatusConverted;
                _context.Update(orderDetail);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("OrdersDetails", new {id = orderId});
        }

        public string GetStatusByValue (string? Status)
        {
            if (Status == "1") return "New";
            if (Status == "2") return "In Progress";
            if (Status == "3") return "Delivered";
            return "";
        }

    }
}
