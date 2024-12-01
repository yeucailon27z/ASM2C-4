using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace ASM2C_4.Models
{
    public class GioHangChiTiet
    {
        public Guid Id { get; set; }
        [ForeignKey("SanPham")]
        public Guid? SanPhamId { get; set; }
        [ForeignKey("GioHang")]
        public Guid? GioHangId { get; set; }
        public int SoLuong { get; set; }
        public decimal ThanhTien {  get; set; }
        public GioHang? GioHang { get; set; }
        public SanPham? SanPham { get; set; }
    }
}
