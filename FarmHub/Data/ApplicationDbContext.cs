using FarmHub.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace FarmHub.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Crop> Crops { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<Report> Reports { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.Entity<Season>()
        .HasOne(s => s.ApplicationUser)
        .WithMany()
        .HasForeignKey(s => s.ApplicationUserId)
        .OnDelete(DeleteBehavior.Cascade);  // Bạn có thể giữ Cascade nếu đây là hành vi mong muốn

        builder.Entity<Season>()
            .HasOne(s => s.Crop)
            .WithMany()
            .HasForeignKey(s => s.CropId)
            .OnDelete(DeleteBehavior.Restrict);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }
}
