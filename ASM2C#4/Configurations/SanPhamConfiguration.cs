using ASM2C_4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ASM2C_4.Configurations
{
    public class SanPhamConfiguration : IEntityTypeConfiguration<SanPham>
    {
        public void Configure(EntityTypeBuilder<SanPham> builder)
        {
            builder.ToTable("SanPhams");

            builder.HasKey(sp => sp.Id);
            builder.Property(sp => sp.Id).ValueGeneratedOnAdd();

            builder.Property(sp => sp.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(sp => sp.ImgUrl)
                .HasMaxLength(250);

            builder.Property(sp => sp.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(sp => sp.Description)
                .HasMaxLength(1000);

            builder.Property(sp => sp.Status)
                .IsRequired();

            builder.HasMany(sp => sp.GHCTs)
                .WithOne(ghct => ghct.SanPham)
                .HasForeignKey(ghct => ghct.SanPhamId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(sp => sp.HoaDonChiTiets)
                .WithOne(hdct => hdct.SanPham)
                .HasForeignKey(hdct => hdct.SanPhamId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
