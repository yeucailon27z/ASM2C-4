using ASM2C_4.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList.Extensions;

namespace ASM2C_4.Controllers
{
    public class SanPhamController : Controller
    {
        ASM2DbContext _db;
        public SanPhamController(ASM2DbContext db)
        {
            _db=db;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult IndexAdmin()
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("LoggedInUser") != null;
            var listSP = _db.SanPhams.ToList();
            return View(listSP);
        }
        public IActionResult IndexKhachHang(int? page)
        {
            ViewBag.IsLoggedIn = HttpContext.Session.GetString("LoggedInUser") != null;
            int pageSize = 9; // Số sản phẩm trên mỗi trang
            int pageNumber = page ?? 1; // Trang hiện tại (mặc định là trang 1)

            var listSP = _db.SanPhams
                .Where(sp => sp.Status == 1)
                .OrderBy(sp => sp.Name)
                .ToPagedList(pageNumber, pageSize);

            return View(listSP);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SanPham sp, IFormFile HinhAnh)
        {
            try
            {
                // Kiểm tra xem người dùng có chọn ảnh hay không
                if (HinhAnh != null && HinhAnh.Length > 0)
                {
                    // Đường dẫn thư mục để lưu ảnh
                    var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

                    // Tạo thư mục nếu chưa tồn tại
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    // Đặt tên file ảnh (có thể đổi tên nếu cần)
                    var fileName = Path.GetFileName(HinhAnh.FileName);

                    // Đặt đường dẫn đầy đủ cho file ảnh
                    var filePath = Path.Combine(folderPath, fileName);

                    // Lưu ảnh vào thư mục
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await HinhAnh.CopyToAsync(fileStream);
                    }

                    // Gán đường dẫn ảnh vào thuộc tính ImgUrl của sản phẩm
                    sp.ImgUrl = "/images/" + fileName;
                }

                // Lưu sản phẩm vào cơ sở dữ liệu
                _db.SanPhams.Add(sp);
                await _db.SaveChangesAsync();

                return RedirectToAction("IndexAdmin");
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }
        public IActionResult Details(Guid id)
        {
            var sp = _db.SanPhams.FirstOrDefault(x => x.Id == id);
            if (sp == null)
            {
                // Nếu không tìm thấy sản phẩm, chuyển hướng đến trang lỗi
                return NotFound();
            }
            return View(sp);
        }
        public IActionResult Edit(Guid id)
        {
            var role = HttpContext.Session.GetString("UserRole");
            if (role != "Admin")
            {
                return Unauthorized(); // Trả về lỗi 401 nếu không phải Admin
            }

            var product = _db.SanPhams.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        public IActionResult Edit(SanPham sp, IFormFile upLoadHinh)
        {
            var editItem = _db.SanPhams.Find(sp.Id);
            if (editItem == null)
            {
                return NotFound();
            }
            editItem.Name = sp.Name;
            editItem.Description = sp.Description;
            editItem.Price = sp.Price;
            if (upLoadHinh != null && upLoadHinh.Length > 0)
            {
                var filePath = Path.Combine("wwwroot/images", upLoadHinh.FileName);

                using var fileStream = new FileStream(filePath, FileMode.Create);
                upLoadHinh.CopyTo(fileStream);

                editItem.ImgUrl = "/images/" + upLoadHinh.FileName;
            }
            try
            {
                _db.SanPhams.Update(editItem);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
            return RedirectToAction("IndexAdmin");
        }
        public IActionResult ChangeStatus(Guid id, byte status)
        {
            var product = _db.SanPhams.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            try
            {
                product.Status = status;
                _db.SanPhams.Update(product);
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                return Content($"Lỗi: {ex.Message}");
            }

            return RedirectToAction("IndexAdmin");
        }
        public IActionResult AddToCart(Guid id, int soLuong)
        {
         
            // Lấy username của người dùng đã đăng nhập (trong trường hợp có session "LoggedInUser")
            var user = HttpContext.Session.GetString("LoggedInUser");
            if (user == null)
            {
                return Content("Chưa đăng nhập hoặc phiên đăng nhập đã hết hạn");
            }

            // Lấy thông tin tài khoản của người dùng
            var acc = _db.Users.FirstOrDefault(x => x.UserName == user);
            if (acc == null)
            {
                return Content("Không tìm thấy tài khoản");
            }
  
            // Lấy giỏ hàng của người dùng
            var gioHang = _db.GioHangs.FirstOrDefault(x => x.UserId == acc.Id);
            if (gioHang == null)
            {
                return Content("Chưa có giỏ hàng. Vui lòng tạo giỏ hàng trước.");
            }

            // Lấy danh sách sản phẩm trong giỏ hàng chi tiết của người dùng
            var accCart = _db.GioHangChiTiets.Where(x => x.GioHangId == gioHang.Id && x.SanPhamId == id).FirstOrDefault();
            var sp = _db.SanPhams.Find(id);
            if (accCart == null)
            {
                // Nếu sản phẩm chưa có trong giỏ hàng, tạo một bản ghi mới
                GioHangChiTiet ghct = new GioHangChiTiet()
                {
                    SanPhamId = id,
                    GioHangId = gioHang.Id,
                    SoLuong = soLuong,
                    ThanhTien = soLuong*sp.Price
                };
                _db.GioHangChiTiets.Add(ghct);
                _db.SaveChanges();
                return RedirectToAction("IndexKhachHang");  // Quay lại trang giỏ hàng
            }
            else
            {
                // Nếu sản phẩm đã có trong giỏ hàng, cộng dồn số lượng
                accCart.SoLuong += soLuong;
                accCart.ThanhTien = accCart.SoLuong * sp.Price;
                _db.GioHangChiTiets.Update(accCart);
                _db.SaveChanges();
                return RedirectToAction("IndexKhachHang");  // Quay lại trang giỏ hàng
            }
        }

    }
}
