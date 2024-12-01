using System.ComponentModel.DataAnnotations;

namespace ASM2C_4.Models
{
    public class SanPham
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tên sản phẩm không được để trống")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm tối đa 200 ký tự")]
        public string Name { get; set; }
        public string? ImgUrl { get; set; }

        [Required(ErrorMessage = "Giá không được để trống")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0")]
        public decimal Price { get; set; }

        public string? Description { get; set; }


        public byte Status { get; set; }

        public ICollection<GioHangChiTiet> GHCTs { get; set; }
        public  ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; }
    }
}
