using System.ComponentModel.DataAnnotations;

namespace ASM2C_4.Models
{
    public class User
    {

        [Key]
        public Guid Id { get; set; } // Sử dụng Guid làm khóa chính

        [Required(ErrorMessage = "Số điện thoại không được để trống")]
        [StringLength(15, ErrorMessage = "Số điện thoại tối đa 15 ký tự")]
        public string SDT { get; set; } // Số điện thoại (có thể là unique)

        [Required(ErrorMessage = "Tên không được để trống")]
        [StringLength(450, MinimumLength = 5, ErrorMessage = "Tên phải có độ dài từ 5 đến 450 ký tự")]
        public string Name { get; set; } // Họ tên đầy đủ

        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Tên đăng nhập phải từ 5 đến 100 ký tự")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự")]
        public string Password { get; set; } // Mật khẩu (khuyến nghị mã hóa trước khi lưu)

        [Required(ErrorMessage = "Vai trò không được để trống")]
        public string Role { get; set; } // Vai trò: "Admin" hoặc "Customer"

        public GioHang? GioHang { get; set; }

        public ICollection<HoaDon> HoaDons { get; set; }
    }
}
