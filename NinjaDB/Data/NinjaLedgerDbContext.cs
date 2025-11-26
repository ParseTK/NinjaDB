using Microsoft.EntityFrameworkCore;
using NinjaDB.Models;

namespace NinjaDB.Data
{
    public partial class NinjaLedgerDbContext : DbContext
    {
        public NinjaLedgerDbContext() { }

        public NinjaLedgerDbContext(DbContextOptions<NinjaLedgerDbContext> options) : base(options) { }

        public virtual DbSet<Customers> Customers { get; set; } = null!;
        public virtual DbSet<Orders> Orders { get; set; } = null!;
        public virtual DbSet<Products> Products { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // This empty override keeps tests working (they pass their own options)
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustomerId);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.Email).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.FirstName).HasMaxLength(50).IsUnicode(false);
                entity.Property(e => e.LastName).HasMaxLength(50).IsUnicode(false);
            });

            modelBuilder.Entity<Orders>(entity =>
            {
                entity.HasKey(e => e.OrderId);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
                entity.HasOne(d => d.Customer)
                      .WithMany(p => p.Orders)
                      .HasForeignKey(d => d.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(d => d.Product)
                      .WithMany(p => p.Orders)
                      .HasForeignKey(d => d.ProductId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Products>(entity =>
            {
                entity.HasKey(e => e.ProductId);
                entity.Property(e => e.ProductName).HasMaxLength(100).IsUnicode(false);
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(10, 2)");
            });
        }
    }
}
