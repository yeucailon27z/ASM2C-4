using ASM2C_4.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace ASM2C_4.Configurations
{
    public class UserConfigurations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users"); // Đặt tên bảng

            builder.HasKey(u => u.Id); // Khóa chính
            builder.Property(u => u.Id).ValueGeneratedOnAdd();

            builder.Property(u => u.SDT)
                .IsRequired()
                .HasMaxLength(15);
            builder.HasIndex(u => u.SDT).IsUnique(); // SDT là duy nhất

            builder.Property(u => u.Name)
                .IsRequired()
                .HasMaxLength(450);

            builder.Property(u => u.UserName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Password)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Role)
                .IsRequired();

            builder.HasOne(u => u.GioHang)
                .WithOne(g => g.User)
                .HasForeignKey<GioHang>(g => g.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(u => u.HoaDons)
                .WithOne(h => h.User)
                .HasForeignKey(h => h.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
