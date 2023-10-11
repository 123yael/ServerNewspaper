using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DAL.Models;

public partial class NewspaperSystemContext : DbContext
{
    public NewspaperSystemContext()
    {
    }

    public NewspaperSystemContext(DbContextOptions<NewspaperSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AdPlacement> AdPlacements { get; set; }

    public virtual DbSet<AdSize> AdSizes { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DatesForOrderDetail> DatesForOrderDetails { get; set; }

    public virtual DbSet<NewspapersPublished> NewspapersPublisheds { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<WordAdSubCategory> WordAdSubCategories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ELIAHU-ASUS-PC\\SQLEXPRESS;Database=NewspaperSystem;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AdPlacement>(entity =>
        {
            entity.HasKey(e => e.PlaceId).HasName("PK__AdPlacem__BF2B684A593D1AD1");

            entity.Property(e => e.PlaceId).HasColumnName("place_id");
            entity.Property(e => e.Img)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("img");
            entity.Property(e => e.PlaceName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("place_name");
            entity.Property(e => e.PlacePrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("place_price");
        });

        modelBuilder.Entity<AdSize>(entity =>
        {
            entity.HasKey(e => e.SizeId).HasName("PK__AdSizes__0DCACE3189DAA17D");

            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.SizeHeight)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("size_height");
            entity.Property(e => e.SizeImg)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("size_img");
            entity.Property(e => e.SizeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("size_name");
            entity.Property(e => e.SizePrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("size_price");
            entity.Property(e => e.SizeWidth)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("size_width");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustId).HasName("PK__Customer__A1B71F9054F3039F");

            entity.Property(e => e.CustId).HasColumnName("cust_id");
            entity.Property(e => e.CustEmail)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cust_email");
            entity.Property(e => e.CustFirstName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cust_firstName");
            entity.Property(e => e.CustLastName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("cust_lastName");
            entity.Property(e => e.CustPassword)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cust_password");
            entity.Property(e => e.CustPhone)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasColumnName("cust_phone");
        });

        modelBuilder.Entity<DatesForOrderDetail>(entity =>
        {
            entity.HasKey(e => e.DateId).HasName("PK__DatesFor__51FC48659EC37726");

            entity.Property(e => e.DateId).HasColumnName("date_id");
            entity.Property(e => e.ApprovalStatus)
                .HasDefaultValueSql("((1))")
                .HasColumnName("approval_status");
            entity.Property(e => e.Date)
                .HasColumnType("date")
                .HasColumnName("date");
            entity.Property(e => e.DetailsId).HasColumnName("details_id");

            entity.HasOne(d => d.Details).WithMany(p => p.DatesForOrderDetails)
                .HasForeignKey(d => d.DetailsId)
                .HasConstraintName("FK__DatesForO__detai__49C3F6B7");
        });

        modelBuilder.Entity<NewspapersPublished>(entity =>
        {
            entity.HasKey(e => e.NewspaperId).HasName("PK__Newspape__710B3F0C99F4C07F");

            entity.ToTable("NewspapersPublished");

            entity.Property(e => e.NewspaperId).HasColumnName("newspaper_id");
            entity.Property(e => e.CountPages).HasColumnName("countPages");
            entity.Property(e => e.PublicationDate)
                .HasColumnType("date")
                .HasColumnName("publication_date");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Orders__465962299F2E70DF");

            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.CustId).HasColumnName("cust_id");
            entity.Property(e => e.OrderDate)
                .HasColumnType("date")
                .HasColumnName("order_date");
            entity.Property(e => e.OrderFinalPrice)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("order_finalPrice");

            entity.HasOne(d => d.Cust).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Orders__cust_id__3A81B327");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.DetailsId).HasName("PK__OrderDet__C3E443F409D3D77B");

            entity.Property(e => e.DetailsId).HasColumnName("details_id");
            entity.Property(e => e.AdContent)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ad_content");
            entity.Property(e => e.AdDuration)
                .HasDefaultValueSql("((1))")
                .HasColumnName("ad_duration");
            entity.Property(e => e.AdFile)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("ad_file");
            entity.Property(e => e.OrderId).HasColumnName("order_id");
            entity.Property(e => e.PlaceId).HasColumnName("place_id");
            entity.Property(e => e.SizeId).HasColumnName("size_id");
            entity.Property(e => e.WordCategoryId).HasColumnName("wordCategory_id");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderDeta__order__4316F928");

            entity.HasOne(d => d.Place).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.PlaceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__place__45F365D3");

            entity.HasOne(d => d.Size).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.SizeId)
                .HasConstraintName("FK__OrderDeta__size___44FF419A");

            entity.HasOne(d => d.WordCategory).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.WordCategoryId)
                .HasConstraintName("FK__OrderDeta__wordC__440B1D61");
        });

        modelBuilder.Entity<WordAdSubCategory>(entity =>
        {
            entity.HasKey(e => e.WordCategoryId).HasName("PK__WordAdSu__2CA95CCDE1666045");

            entity.Property(e => e.WordCategoryId).HasColumnName("wordCategory_id");
            entity.Property(e => e.WordCategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("wordCategory_name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
