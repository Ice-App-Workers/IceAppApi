using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace IceAppApi.Models
{
    public partial class IceAppDbContext : DbContext
    {
        public IceAppDbContext()
        {
        }

        public IceAppDbContext(DbContextOptions<IceAppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<IceKind> IceKinds { get; set; }
        public virtual DbSet<IceShopOffer> IceShopOffers { get; set; }
        public virtual DbSet<IceShopOwner> IceShopOwners { get; set; }
        public virtual DbSet<IceTaste> IceTastes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS;Database=IceAppDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Polish_CI_AS");

            modelBuilder.Entity<IceKind>(entity =>
            {
                entity.ToTable("IceKind");

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.IceKind1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("IceKind");
            });

            modelBuilder.Entity<IceShopOffer>(entity =>
            {
                entity.ToTable("IceShopOffer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IceKindNavigation)
                    .WithMany(p => p.IceShopOffers)
                    .HasForeignKey(d => d.IceKind)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IceShopOffer_IceKind");

                entity.HasOne(d => d.IceShopOwnerNavigation)
                    .WithMany(p => p.IceShopOffers)
                    .HasForeignKey(d => d.IceShopOwner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IceShopOffer_IceShopOwner");

                entity.HasOne(d => d.IceTasteNavigation)
                    .WithMany(p => p.IceShopOffers)
                    .HasForeignKey(d => d.IceTaste)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_IceShopOffer_IceTaste");
            });

            modelBuilder.Entity<IceShopOwner>(entity =>
            {
                entity.ToTable("IceShopOwner");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Lat)
                    .HasColumnType("numeric(20, 16)")
                    .HasColumnName("lat");

                entity.Property(e => e.Lng)
                    .HasColumnType("numeric(20, 16)")
                    .HasColumnName("lng");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Number)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.PostalCode)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Street)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserId).HasColumnName("UserID");
            });

            modelBuilder.Entity<IceTaste>(entity =>
            {
                entity.ToTable("IceTaste");

                entity.Property(e => e.Id)
                    .HasColumnName("ID")
                    .HasDefaultValueSql("(newid())");

                entity.Property(e => e.IceTaste1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("IceTaste");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
