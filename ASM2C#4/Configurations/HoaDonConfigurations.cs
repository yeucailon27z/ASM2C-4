using ASM2C_4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ASM2C_4.Configurations
{
    public class HoaDonConfigurations : IEntityTypeConfiguration<HoaDon>
    {
        public void Configure(EntityTypeBuilder<HoaDon> builder)
        {
            builder.ToTable("HoaDons");

            builder.HasKey(hd => hd.Id);
            builder.Property(hd => hd.Id).ValueGeneratedOnAdd();

            builder.Property(hd => hd.SoldDate)
                .IsRequired(false);

            builder.Property(hd => hd.TotalMoney)
                .HasColumnType("decimal(18,2)");

            builder.Property(hd => hd.Status)
                .HasMaxLength(100);

            builder.HasMany(hd => hd.HoaDonChiTiets)
                .WithOne(hdct => hdct.HoaDon)
                .HasForeignKey(hdct => hdct.HoaDonId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
