using ASM2C_4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ASM2C_4.Configurations
{
    public class HoaDonChiTietConfiguration : IEntityTypeConfiguration<HoaDonChiTiet>
    {
        public void Configure(EntityTypeBuilder<HoaDonChiTiet> builder)
        {
            builder.ToTable("HoaDonChiTiets");

            builder.HasKey(hdct => hdct.Id);
            builder.Property(hdct => hdct.Id).ValueGeneratedOnAdd();

            builder.Property(hdct => hdct.ToTalMoney)
                .HasColumnType("decimal(18,2)");

            builder.HasOne(hdct => hdct.SanPham)
                .WithMany(sp => sp.HoaDonChiTiets)
                .HasForeignKey(hdct => hdct.SanPhamId);

            builder.HasOne(hdct => hdct.HoaDon)
                .WithMany(hd => hd.HoaDonChiTiets)
                .HasForeignKey(hdct => hdct.HoaDonId);
        }
    }

}
