using System.ComponentModel.DataAnnotations.Schema;

namespace ASM2C_4.Models
{
    public class GioHang
    {
        public Guid Id { get; set; }
        public string? UserName { get; set; }
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<GioHangChiTiet> GioHangChiTiets { get; set; }
    }
}
