using ASM2C_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ASM2C_4.Controllers
{
    public class GioHangController : Controller
    {
        private readonly ASM2DbContext _db;

        public GioHangController(ASM2DbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            // Lấy tài khoản đăng nhập từ Session
            var acc = HttpContext.Session.GetString("LoggedInUser");
            var getUser = _db.Users.FirstOrDefault(x => x.UserName == acc);

            // Kiểm tra nếu không tìm thấy user
            if (getUser == null)
            {
                ViewBag.CartCount = 0; // Không có giỏ hàng
                return View(new List<GioHangChiTiet>());
            }

            // Lấy giỏ hàng tương ứng
            var gh = _db.GioHangs.FirstOrDefault(x => x.UserId == getUser.Id);

            // Kiểm tra nếu không có giỏ hàng
            if (gh == null)
            {
                ViewBag.CartCount = 0; // Giỏ hàng trống
                return View(new List<GioHangChiTiet>());
            }

            // Lấy dữ liệu giỏ hàng chi tiết và bao gồm sản phẩm liên quan
            var data = _db.GioHangChiTiets
                          .Where(x => x.GioHangId == gh.Id)
                          .Include(x => x.SanPham) // Đảm bảo lấy sản phẩm kèm theo
                          .ToList();

            // Tính tổng số lượng sản phẩm trong giỏ hàng
            var totalItems = data.Sum(x => x.SoLuong);
            ViewBag.CartCount = totalItems;

            // Trả về view với danh sách giỏ hàng chi tiết
            return View(data);
        }

        [HttpPost]
        public IActionResult UpdateQuantity(Guid id, int newQuantity)
        {
            var gioHangCT = _db.GioHangChiTiets.FirstOrDefault(x => x.Id == id);

            if (gioHangCT != null)
            {
                gioHangCT.SoLuong = newQuantity;
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(Guid id)
        {
            var gioHangCT = _db.GioHangChiTiets.FirstOrDefault(x => x.Id == id);

            if (gioHangCT != null)
            {
                _db.GioHangChiTiets.Remove(gioHangCT);
                _db.SaveChanges();
            }

            return RedirectToAction("Index");
        }
        public IActionResult Checkout()
        {
            var acc = HttpContext.Session.GetString("LoggedInUser");
            if (string.IsNullOrEmpty(acc))
            {
                return RedirectToAction("Login", "Account");
            }

            var getUser = _db.Users.FirstOrDefault(x => x.UserName == acc);
            var gh = _db.GioHangs.FirstOrDefault(x => x.UserId == getUser.Id);
            var data = _db.GioHangChiTiets.Where(x => x.GioHangId == gh.Id).ToList();

            return View(data);
        }

    }
}
