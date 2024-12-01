using ASM2C_4.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace ASM2C_4.Controllers
{
    public class UserController : Controller
    {
        private readonly ASM2DbContext _context;

        public UserController(ASM2DbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult DangKy(User user)
        {
            try
            {
                // Kiểm tra nếu user đã tồn tại
                if (_context.Users.Any(x => x.UserName == user.UserName))
                {
                    TempData["ErrorMessage"] = "Tài khoản đã tồn tại.";
                    return View(user);
                }

                user.Role = "KhachHang";
                _context.Users.Add(user);
                _context.SaveChanges();

                // Tạo giỏ hàng cho người dùng mới
                GioHang gioHang = new GioHang()
                {
                    UserName = user.UserName,
                    UserId = user.Id, // Giỏ hàng cần liên kết với ID người dùng
                };

                _context.GioHangs.Add(gioHang);
                _context.SaveChanges();

                TempData["Status"] = "Chúc mừng bạn đã tạo tài khoản thành công!";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi tạo tài khoản: {ex.Message}";
                return View(user);
            }

        }
        public IActionResult Login()
        {
            return View();
        }
        //xử lí logic của login
        [HttpPost]
        public IActionResult Login(string userName, string passWord)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(passWord))
            {
                TempData["ErrorMessage"] = "Vui lòng nhập tài khoản và mật khẩu.";
                return View();
            }
            if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(passWord))
            {
                ViewBag.ErrorMessage = "Tên đăng nhập và mật khẩu không được để trống.";
                return View();
            }
            //tìm kiếm xem thông tin userName và pass có tồn tại trong csdl
            var acc = _context.Users.ToList()
                .FirstOrDefault(x => x.UserName == userName && x.Password == passWord);
            if (acc == null)
            {
                ViewBag.ErrorMessage = "Tài khoản hoặc mật khẩu chưa chính xác.";
                return View();
            }
            else
            {
                HttpContext.Session.SetString("UserRole", acc.Role); // "Admin" hoặc "User"
                HttpContext.Session.SetString("LoggedInUser", userName);
                if (acc.Role == "Admin")
                {
                    return RedirectToAction("IndexAdmin", "SanPham");
                }
                else
                {
                    return RedirectToAction("IndexKhachHang", "SanPham");
                }
            }
        }
      
            public IActionResult Logout()
            {
                // Xóa dữ liệu phiên làm việc
                HttpContext.Session.Clear();

                // Chuyển hướng về trang chủ hoặc trang đăng nhập
                return RedirectToAction("Login", "User");
            }

    }
}
