using System.ComponentModel.DataAnnotations.Schema;

namespace ASM2C_4.Models
{
    public class HoaDonChiTiet
    {
        public Guid Id { get; set; }

        [ForeignKey("SanPham")]
        public Guid? SanPhamId { get; set; }

        [ForeignKey("HoaDon")]
        public Guid? HoaDonId { get; set; }

        public decimal? ToTalMoney { get; set; }

        public SanPham? SanPham { get; set; }
        public HoaDon? HoaDon { get; set; }
    }

}
