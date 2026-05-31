using Microsoft.EntityFrameworkCore;
using rut_shop.net.model;

namespace rut_shop.net.database;

public class CloudPlatformDbContext(DbContextOptions<CloudPlatformDbContext> options) : DbContext(options)
{
    public DbSet<ComputingPackage> ComputingPackages { get; set; }
    public DbSet<Company> Companies { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ComputingPackage>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(200).IsRequired();
            entity.Property(x => x.PricePerMonth).HasPrecision(18, 2);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CompanyName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.ContactEmail).HasMaxLength(300).IsRequired();
        });

        modelBuilder.Entity<Subscription>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.CompanyName).HasMaxLength(200).IsRequired();
            entity.Property(x => x.BillingAmount).HasPrecision(18, 2);
            entity.Property(x => x.DiscountApplied).HasPrecision(18, 2);
            entity.Property(x => x.Status).HasMaxLength(50).IsRequired();
        });
    }
}
