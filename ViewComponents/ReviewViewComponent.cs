using Microsoft.AspNetCore.Mvc;
using WebsiteTMDT.Data;
using WebsiteTMDT.ViewModels;
using System.Linq;

namespace WebsiteTMDT.ViewComponents
{
    public class ReviewViewComponent : ViewComponent
    {
        private readonly WebsiteContext db;

        public ReviewViewComponent(WebsiteContext context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Reviews.Select(rv => new ReviewVM
            {
                maReview = rv.ReviewId,
                maKhachHang = rv.UserId,
                tenKhachHang = rv.User.FullName,
                maSanPham = rv.ProductId,
                danhGia = rv.Rating,
                noiDung = rv.ReviewText,
                ngayDang = rv.CreatedAt.HasValue ? rv.CreatedAt.Value.ToString("dd/MM/yyyy") : "Chưa cập nhật"
            }).ToList();

            return View(data);
        }
    }
}
