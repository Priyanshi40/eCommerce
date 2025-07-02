using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public class E_CommerceContext : IdentityDbContext<IdentityUser>
{
    public E_CommerceContext()
    {
    }
    public E_CommerceContext(DbContextOptions<E_CommerceContext> options)
        : base(options)
    {
    }
    public DbSet<UserDetails> UserDetails { get; set; }
    public virtual DbSet<Address> Addresses { get; set; }
    public virtual DbSet<VendorDetails> VendorDetails { get; set; }
    public virtual DbSet<Category> Category { get; set; }
    public virtual DbSet<Order> Orders { get; set; }
    public virtual DbSet<OrderProduct> OrderProduct { get; set; }
    public virtual DbSet<Product> Product { get; set; }
    public virtual DbSet<ProductImage> ProductImage { get; set; }
    public virtual DbSet<Wishlist> Wishlist { get; set; }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql("Host=localhost; Database=Ecommerce; Username=postgres;password=Tatva@123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<UserDetails>(entity =>
        {
            entity.ToTable("UserDetails");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");

            entity.HasOne(d => d.IUser).WithMany()
                .HasForeignKey(d => d.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("identity_user_fkey");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Category");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CreatedAt");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("ModifiedAt");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Product");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CreatedAt");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("ModifiedAt");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Order");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("OrderDate");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("CreatedAt");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("ModifiedAt");
        });
    }
}