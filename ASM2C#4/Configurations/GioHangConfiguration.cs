using ASM2C_4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ASM2C_4.Configurations
{
    public class GioHangConfiguration : IEntityTypeConfiguration<GioHang>
    {
        public void Configure(EntityTypeBuilder<GioHang> builder)
        {
            builder.ToTable("GioHangs");

            builder.HasKey(gh => gh.Id);
            builder.Property(gh => gh.Id).ValueGeneratedOnAdd();

            builder.Property(gh => gh.UserName)
                .HasMaxLength(100);

            builder.HasMany(gh => gh.GioHangChiTiets)
                .WithOne(ghct => ghct.GioHang)
                .HasForeignKey(ghct => ghct.GioHangId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
