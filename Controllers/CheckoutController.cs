using Microsoft.AspNetCore.Mvc;
using WebsiteTMDT.Data;
using WebsiteTMDT.Helpers;
using WebsiteTMDT.ViewModels;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebsiteTMDT.Controllers
{
    [Authorize(Roles = "Customer")]
    public class CheckoutController : Controller
    {
        private readonly WebsiteContext _context;

        public CheckoutController(WebsiteContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();
            if (!cart.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn đang trống!";
                return RedirectToAction("Index", "Cart");
            }
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessOrder()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY);
            if (cart == null || !cart.Any())
            {
                TempData["Error"] = "Giỏ hàng của bạn trống!";
                return RedirectToAction("Index", "Cart");
            }

            var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value ?? "0");
            var totalAmount = cart.Sum(item => item.SoLuong * (decimal)item.Gia);

            var order = new Order
            {
                UserId = userId,
                TotalAmount = totalAmount,
                Status = "Pending",
                CreatedAt = DateTime.Now,
                OrderDetails = cart.Select(item => new OrderDetail
                {
                    ProductId = item.MaSP,
                    Quantity = item.SoLuong,
                    Price = (decimal)item.Gia
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            HttpContext.Session.Remove(MySetting.CART_KEY);

            return RedirectToAction("OrderConfirmation", new { orderId = order.OrderId });
        }

        public async Task<IActionResult> OrderConfirmation(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderDetails)
                .ThenInclude(od => od.Product)
                .FirstOrDefaultAsync(o => o.OrderId == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
    }
}
