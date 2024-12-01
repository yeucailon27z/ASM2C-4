using ASM2C_4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ASM2C_4.Configurations
{
    public class GioHangChiTietConfiguration : IEntityTypeConfiguration<GioHangChiTiet>
    {
        public void Configure(EntityTypeBuilder<GioHangChiTiet> builder)
        {
            builder.ToTable("GioHangChiTiets");

            builder.HasKey(ghct => ghct.Id);
            builder.Property(ghct => ghct.Id).ValueGeneratedOnAdd();

            builder.Property(ghct => ghct.SoLuong)
                .IsRequired();

            builder.Property(ghct => ghct.ThanhTien)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.HasOne(ghct => ghct.SanPham)
                .WithMany(sp => sp.GHCTs)
                .HasForeignKey(ghct => ghct.SanPhamId);

            builder.HasOne(ghct => ghct.GioHang)
                .WithMany(gh => gh.GioHangChiTiets)
                .HasForeignKey(ghct => ghct.GioHangId);
        }
    }

}
