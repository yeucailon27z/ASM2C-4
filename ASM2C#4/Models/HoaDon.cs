using System.ComponentModel.DataAnnotations.Schema;

namespace ASM2C_4.Models
{
    public class HoaDon
    {
        public Guid Id { get; set; }
        public DateTime? SoldDate { get; set; }
        public decimal? TotalMoney { get; set; }
        public string? Status { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public ICollection<HoaDonChiTiet> HoaDonChiTiets { get; set; }
    }
}
