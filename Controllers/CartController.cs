using WebsiteTMDT.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebsiteTMDT.Data;
using WebsiteTMDT.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebsiteTMDT.Controllers
{
    public class CartController : Controller
    {
        private readonly WebsiteContext db;

        public CartController(WebsiteContext context)
        {
            db = context;
        }

        public List<CartItem> Cart => HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

        public IActionResult Index()
        {
            return View(Cart);
        }

        [Authorize(Roles = "Customer")]
        public IActionResult AddToCart(int id, int quantity = 1)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaSP == id);
            if (item == null)
            {
                var hangHoa = db.Products.SingleOrDefault(p => p.ProductId == id);
                if (hangHoa == null)
                {
                    TempData["Error"] = "Sản phẩm không tồn tại";
                    return Redirect("/404");
                }
                item = new CartItem
                {
                    MaSP = hangHoa.ProductId,
                    TenSP = hangHoa.ProductName,
                    Gia = (double)hangHoa.Price,
                    HinhAnh = hangHoa.ImageUrl,
                    SoLuong = quantity
                };
                gioHang.Add(item);
            }
            else
            {
                item.SoLuong += quantity;
            }

            HttpContext.Session.Set(MySetting.CART_KEY, gioHang);

            return RedirectToAction("Index", "Product");
        }

        public IActionResult RemoveCart(int id)
        {
            var gioHang = Cart;
            var item = gioHang.SingleOrDefault(p => p.MaSP == id);
            if (item != null)
            {
                gioHang.Remove(item);
                HttpContext.Session.Set(MySetting.CART_KEY, gioHang);
            }
            return RedirectToAction("Index");
        }
    }
}
