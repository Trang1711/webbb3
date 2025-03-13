using WebsiteTMDT.Helpers;
using WebsiteTMDT.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace WebsiteTMDT.ViewComponents
{
    public class CartQuickViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>(MySetting.CART_KEY) ?? new List<CartItem>();

            return View(cart); 
        }
    }
}
