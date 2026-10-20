using System;
using System.Collections.Generic;
using Ecommerce.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Infrastructure.Dbcontext;

public partial class EcommerceDbContext : DbContext
{
    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationPermission> ApplicationPermissions { get; set; }

    public virtual DbSet<AuditLog> AuditLogs { get; set; }

    public virtual DbSet<Brand> Brands { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ColorImage> ColorImages { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<CustomerAddress> CustomerAddresses { get; set; }

    public virtual DbSet<CustomerTier> CustomerTiers { get; set; }

    public virtual DbSet<CustomerWishlist> CustomerWishlists { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    public virtual DbSet<InventoryTransactionItem> InventoryTransactionItems { get; set; }

    public virtual DbSet<MainOrder> MainOrders { get; set; }

    public virtual DbSet<MainOrderItem> MainOrderItems { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderItem> OrderItems { get; set; }

    public virtual DbSet<OrderUpdateItem> OrderUpdateItems { get; set; }

    public virtual DbSet<OrderUpdateRequest> OrderUpdateRequests { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductColor> ProductColors { get; set; }

    public virtual DbSet<ProductSpecification> ProductSpecifications { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleClaim> RoleClaims { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserClaim> UserClaims { get; set; }

    public virtual DbSet<UserProfile> UserProfiles { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WorkLocation> WorkLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ApplicationPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Applicat__3214EC07B8236448");

            entity.HasIndex(e => e.PermissionKey, "UQ__Applicat__8884ABD4EFAFE1B7").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.GroupName).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.PermissionKey)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AuditLog__3214EC07A1D32A9D");

            entity.HasIndex(e => e.ChangedAt, "IX_AuditLogs_ChangedAt");

            entity.HasIndex(e => new { e.EntityName, e.EntityId }, "IX_AuditLogs_EntityName_EntityId");

            entity.Property(e => e.Action)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ChangedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ChangedBy).HasMaxLength(100);
            entity.Property(e => e.EntityId)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.EntityName).HasMaxLength(100);
            entity.Property(e => e.IpAddress)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("");
        });

        modelBuilder.Entity<Brand>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Brands__3214EC0745D6F113");

            entity.HasIndex(e => new { e.IsActive, e.IsDelete }, "IX_Brands_IsActive_IsDelete");

            entity.HasIndex(e => e.Name, "IX_Brands_Name");

            entity.HasIndex(e => e.Slug, "IX_Brands_Slug");

            entity.HasIndex(e => e.Slug, "UQ__Brands__BC7B5FB6CDC50DBC").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.LogoUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Slug)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC07EEDC7B28");

            entity.HasIndex(e => new { e.IsActive, e.IsDelete }, "IX_Categories_IsActive_IsDelete");

            entity.HasIndex(e => e.Name, "IX_Categories_Name");

            entity.HasIndex(e => e.ParentId, "IX_Categories_ParentId");

            entity.HasIndex(e => e.Slug, "IX_Categories_Slug");

            entity.HasIndex(e => e.Slug, "UQ__Categori__BC7B5FB6A8C16F27").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Slug)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Categories_Parent");
        });

        modelBuilder.Entity<ColorImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ColorIma__3214EC07B7A70295");

            entity.HasIndex(e => e.ColorId, "IX_ColorImages_ColorId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsThumbnail).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Color).WithMany(p => p.ColorImages)
                .HasForeignKey(d => d.ColorId)
                .HasConstraintName("FK_ColorImages_Color");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07B3D11BB1");

            entity.HasIndex(e => e.CustomerTierId, "IX_Customers_CustomerTierId");

            entity.HasIndex(e => e.Email, "IX_Customers_Email");

            entity.HasIndex(e => e.FullName, "IX_Customers_FullName");

            entity.HasIndex(e => new { e.IsActive, e.IsDelete }, "IX_Customers_IsActive_IsDelete");

            entity.HasIndex(e => e.Phone, "IX_Customers_Phone");

            entity.HasIndex(e => e.Phone, "UQ__Customer__5C7E359E7B71582F").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Customer__A9D10534C261E53C").IsUnique();

            entity.Property(e => e.AvatarUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(150);
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.LoyaltyPoints).HasDefaultValue(0);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.TotalSpent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CustomerTier).WithMany(p => p.Customers)
                .HasForeignKey(d => d.CustomerTierId)
                .HasConstraintName("FK_Customers_Tier");
        });

        modelBuilder.Entity<CustomerAddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC072B995B6D");

            entity.HasIndex(e => e.CustomerId, "IX_CustomerAddresses_CustomerId");

            entity.HasIndex(e => e.IsDefault, "IX_CustomerAddresses_IsDefault");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DetailAddress).HasMaxLength(255);
            entity.Property(e => e.DistrictName).HasMaxLength(100);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProvinceName).HasMaxLength(100);
            entity.Property(e => e.ReceiverName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.WardName).HasMaxLength(100);

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerAddresses)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_CustomerAddresses_Customer");
        });

        modelBuilder.Entity<CustomerTier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC07CC2DF1F4");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountPercent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.MinSpent).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<CustomerWishlist>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0722FF8CFF");

            entity.HasIndex(e => e.CustomerId, "IX_CustomerWishlists_CustomerId");

            entity.HasIndex(e => e.ProductId, "IX_CustomerWishlists_ProductId");

            entity.HasIndex(e => new { e.CustomerId, e.ProductId }, "UQ_Customer_Product").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.CustomerWishlists)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Wishlists_Customer");

            entity.HasOne(d => d.Product).WithMany(p => p.CustomerWishlists)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Wishlists_Product");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC07240995FF");

            entity.ToTable("Inventory");

            entity.HasIndex(e => e.ProductVariantId, "IX_Inventory_ProductVariantId");

            entity.HasIndex(e => e.WarehouseId, "IX_Inventory_WarehouseId");

            entity.HasIndex(e => new { e.WarehouseId, e.ProductVariantId }, "UQ_Warehouse_Variant").IsUnique();

            entity.Property(e => e.AvailableQuantity).HasComputedColumnSql("([Quantity]-[ReservedQuantity])", false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.MaxStockLevel).HasDefaultValue(1000);
            entity.Property(e => e.MinStockLevel).HasDefaultValue(5);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK_Inventory_Variant");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK_Inventory_Warehouse");
        });

        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC07B09D3E88");

            entity.HasIndex(e => e.TransactionCode, "IX_InvTrans_TransactionCode");

            entity.HasIndex(e => e.TransactionType, "IX_InvTrans_TransactionType");

            entity.HasIndex(e => e.CreatedByUserId, "IX_InventoryTransactions_CreatedByUserId");

            entity.HasIndex(e => e.DestinationWarehouseId, "IX_InventoryTransactions_DestinationWarehouseId");

            entity.HasIndex(e => e.SourceWarehouseId, "IX_InventoryTransactions_SourceWarehouseId");

            entity.HasIndex(e => e.Status, "IX_InventoryTransactions_Status");

            entity.HasIndex(e => e.TransactionCode, "UQ__Inventor__D85E70268F9C59EE").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Draft");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.CreatedByUser).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.CreatedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvTrans_User");

            entity.HasOne(d => d.DestinationWarehouse).WithMany(p => p.InventoryTransactionDestinationWarehouses)
                .HasForeignKey(d => d.DestinationWarehouseId)
                .HasConstraintName("FK_InvTrans_DestWH");

            entity.HasOne(d => d.SourceWarehouse).WithMany(p => p.InventoryTransactionSourceWarehouses)
                .HasForeignKey(d => d.SourceWarehouseId)
                .HasConstraintName("FK_InvTrans_SourceWH");
        });

        modelBuilder.Entity<InventoryTransactionItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC079C2B6017");

            entity.HasIndex(e => e.ProductVariantId, "IX_InventoryTransactionItems_ProductVariantId");

            entity.HasIndex(e => e.TransactionId, "IX_InventoryTransactionItems_TransactionId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Note).HasMaxLength(250);
            entity.Property(e => e.TotalPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.InventoryTransactionItems)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvTransItem_Variant");

            entity.HasOne(d => d.Transaction).WithMany(p => p.InventoryTransactionItems)
                .HasForeignKey(d => d.TransactionId)
                .HasConstraintName("FK_InvTransItem_Trans");
        });

        modelBuilder.Entity<MainOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MainOrde__3214EC0719FC5B4C");

            entity.HasIndex(e => e.CreatedAt, "IX_MainOrders_CreatedAt");

            entity.HasIndex(e => e.CustomerId, "IX_MainOrders_CustomerId");

            entity.HasIndex(e => e.OrderStatus, "IX_MainOrders_OrderStatus");

            entity.HasIndex(e => e.OrderCode, "UQ__MainOrde__999B5229A3D68B07").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName).HasMaxLength(150);
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FulfillmentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Delivery");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderSource)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("WebEcom");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("PendingConfirmation");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ReceiverName).HasMaxLength(100);
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.Property(e => e.ShippingFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.UserFullName).HasMaxLength(150);
            entity.Property(e => e.UserPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkLocationAddress).HasMaxLength(300);
            entity.Property(e => e.WorkLocationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkLocationName).HasMaxLength(150);
        });

        modelBuilder.Entity<MainOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__MainOrde__3214EC0769732BA6");

            entity.HasIndex(e => e.OrderId, "IX_MainOrderItems_OrderId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ColorHex)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ColorName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SKU");
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VariantName).HasMaxLength(250);

            entity.HasOne(d => d.Order).WithMany(p => p.MainOrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_MainOrderItems_MainOrder");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Orders__3214EC079482B810");

            entity.HasIndex(e => e.CreatedAt, "IX_Orders_CreatedAt");

            entity.HasIndex(e => e.CustomerId, "IX_Orders_CustomerId");

            entity.HasIndex(e => e.CustomerPhone, "IX_Orders_CustomerPhone");

            entity.HasIndex(e => e.IsDelete, "IX_Orders_IsDelete");

            entity.HasIndex(e => e.OrderCode, "IX_Orders_OrderCode");

            entity.HasIndex(e => e.OrderStatus, "IX_Orders_OrderStatus");

            entity.HasIndex(e => e.PaymentMethod, "IX_Orders_PaymentMethod");

            entity.HasIndex(e => e.PaymentStatus, "IX_Orders_PaymentStatus");

            entity.HasIndex(e => new { e.OrderStatus, e.CreatedAt }, "IX_Orders_Status_Created").IsDescending(false, true);

            entity.HasIndex(e => e.UserId, "IX_Orders_UserId");

            entity.HasIndex(e => e.WorkLocationId, "IX_Orders_WorkLocationId");

            entity.HasIndex(e => e.OrderCode, "UQ__Orders__999B5229A0A2EEBF").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName).HasMaxLength(150);
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FulfillmentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Delivery");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderSource)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("WebEcom");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("PendingConfirmation");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ReceiverName).HasMaxLength(100);
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.Property(e => e.ShippingFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.UserFullName).HasMaxLength(150);
            entity.Property(e => e.UserPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkLocationAddress).HasMaxLength(300);
            entity.Property(e => e.WorkLocationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkLocationName).HasMaxLength(150);
        });

        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderIte__3214EC0748229ACB");

            entity.HasIndex(e => e.OrderId, "IX_OrderItems_OrderId");

            entity.HasIndex(e => e.ProductVariantId, "IX_OrderItems_ProductVariantId");

            entity.Property(e => e.ColorHex)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ColorName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SKU");
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VariantName).HasMaxLength(250);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderItems_Order");
        });

        modelBuilder.Entity<OrderUpdateItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderUpd__3214EC0795DB3B7F");

            entity.HasIndex(e => e.OrderId, "IX_OrderUpdateItems_OrderId");

            entity.Property(e => e.ColorHex)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ColorName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ProductName).HasMaxLength(250);
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SKU");
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TotalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.VariantName).HasMaxLength(250);

            entity.HasOne(d => d.Order).WithMany(p => p.OrderUpdateItems)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderUpdateItems_Order");
        });

        modelBuilder.Entity<OrderUpdateRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__OrderUpd__3214EC0750B6C363");

            entity.HasIndex(e => e.CustomerId, "IX_OrderUpdateRequests_CustomerId");

            entity.HasIndex(e => e.Status, "IX_OrderUpdateRequests_Status");

            entity.HasIndex(e => e.OrderCode, "UQ__OrderUpd__999B5229BA51FE12").IsUnique();

            entity.Property(e => e.ApprovalNote).HasMaxLength(500);
            entity.Property(e => e.ApprovedAt).HasColumnType("datetime");
            entity.Property(e => e.ApprovedByUserName).HasMaxLength(150);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerEmail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CustomerName).HasMaxLength(150);
            entity.Property(e => e.CustomerPhone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.FulfillmentType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Delivery");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.OrderCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.OrderSource)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("WebEcom");
            entity.Property(e => e.OrderStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("PendingConfirmation");
            entity.Property(e => e.PaymentMethod)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.ReceiverName).HasMaxLength(100);
            entity.Property(e => e.RequestNote).HasMaxLength(500);
            entity.Property(e => e.RequestType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.Property(e => e.ShippingFee)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Pending");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserEmail)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.UserFullName).HasMaxLength(150);
            entity.Property(e => e.UserPhoneNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkLocationAddress).HasMaxLength(300);
            entity.Property(e => e.WorkLocationCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.WorkLocationName).HasMaxLength(150);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC0729488B53");

            entity.HasIndex(e => e.BrandId, "IX_Products_BrandId");

            entity.HasIndex(e => new { e.BrandId, e.Status, e.CreatedAt }, "IX_Products_Brand_Status_Created").IsDescending(false, false, true);

            entity.HasIndex(e => e.IsDelete, "IX_Products_IsDelete");

            entity.HasIndex(e => e.IsFeatured, "IX_Products_IsFeatured");

            entity.HasIndex(e => e.Name, "IX_Products_Name");

            entity.HasIndex(e => e.Slug, "IX_Products_Slug");

            entity.HasIndex(e => e.Status, "IX_Products_Status");

            entity.HasIndex(e => e.Slug, "UQ__Products__BC7B5FB60AFECC95").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsFeatured).HasDefaultValue(false);
            entity.Property(e => e.MetaDescription).HasMaxLength(500);
            entity.Property(e => e.MetaTitle).HasMaxLength(150);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Slug)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.Summary).HasMaxLength(500);
            entity.Property(e => e.ThumbnailUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Brand).WithMany(p => p.Products)
                .HasForeignKey(d => d.BrandId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Products_Brand");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => new { e.ProductId, e.CategoryId });

            entity.HasIndex(e => e.CategoryId, "IX_ProductCategories_CategoryId");

            entity.HasIndex(e => e.IsPrimary, "IX_ProductCategories_IsPrimary");

            entity.HasIndex(e => e.ProductId, "IX_ProductCategories_ProductId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.IsPrimary).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_PC_Category");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductCategories)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_PC_Product");
        });

        modelBuilder.Entity<ProductColor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductC__3214EC07DAF3AF91");

            entity.HasIndex(e => e.ProductId, "IX_ProductColors_ProductId");

            entity.Property(e => e.ColorHex)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.ColorName).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductColors)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_ProductColors_Product");
        });

        modelBuilder.Entity<ProductSpecification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductS__3214EC076587F2FE");

            entity.HasIndex(e => e.FilterKey, "IX_ProductSpecifications_FilterKey");

            entity.HasIndex(e => new { e.FilterKey, e.DisplayValue }, "IX_ProductSpecifications_FilterKey_Value");

            entity.HasIndex(e => e.NumericValue, "IX_ProductSpecifications_NumericValue");

            entity.HasIndex(e => e.ProductVariantId, "IX_ProductSpecifications_ProductVariantId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DisplayOrder).HasDefaultValue(0);
            entity.Property(e => e.DisplayValue).HasMaxLength(255);
            entity.Property(e => e.FilterKey)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.GroupName).HasMaxLength(100);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.NumericValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SpecKey).HasMaxLength(100);
            entity.Property(e => e.Unit)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductSpecifications)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK_Specs_ProductVariant");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC07B3B1E880");

            entity.HasIndex(e => e.ColorId, "IX_ProductVariants_ColorId");

            entity.HasIndex(e => e.IsDelete, "IX_ProductVariants_IsDelete");

            entity.HasIndex(e => e.ProductId, "IX_ProductVariants_ProductId");

            entity.HasIndex(e => e.Sku, "IX_ProductVariants_SKU");

            entity.HasIndex(e => e.SellingPrice, "IX_ProductVariants_SellingPrice");

            entity.HasIndex(e => e.Slug, "IX_ProductVariants_Slug");

            entity.HasIndex(e => e.Status, "IX_ProductVariants_Status");

            entity.HasIndex(e => e.Slug, "UQ__ProductV__BC7B5FB6EE2C78A3").IsUnique();

            entity.HasIndex(e => e.Sku, "UQ__ProductV__CA1ECF0D728AF934").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.IsAllowStackingPromo).HasDefaultValue(false);
            entity.Property(e => e.IsDefault).HasDefaultValue(false);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.OriginalPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SalePrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SellingPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("SKU");
            entity.Property(e => e.Slug)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Active");
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Color).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ColorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Variants_Color");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Variants_Product");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC07B742E8DF");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F6570E80D7").IsUnique();

            entity.HasIndex(e => e.NormalizedName, "UQ__Roles__A93C97B93BFC7DF4").IsUnique();

            entity.HasIndex(e => e.NameDisplay, "UQ__Roles__DEBC6BDE574B7C29").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.NameDisplay).HasMaxLength(256);
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<RoleClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleClai__3214EC075901075D");

            entity.HasIndex(e => e.RoleId, "IX_RoleClaims_RoleId");

            entity.Property(e => e.ClaimType)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleClaims)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_RoleClaims_Role");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC071AFA9E70");

            entity.HasIndex(e => e.IsDelete, "IX_Users_IsActive_IsDelete");

            entity.HasIndex(e => e.WorkLocationId, "IX_Users_WorkLocationId");

            entity.HasIndex(e => e.NormalizedUserName, "UQ__Users__54E8BE222332EB5D").IsUnique();

            entity.HasIndex(e => e.UserName, "UQ__Users__C9F284569F173524").IsUnique();

            entity.Property(e => e.AccessFailedCount).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.LockoutEnabled).HasDefaultValue(true);
            entity.Property(e => e.LockoutEnd).HasColumnType("datetime");
            entity.Property(e => e.NormalizedUserName)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TwoFactorEnabled).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            entity.Property(e => e.UserName)
                .HasMaxLength(256)
                .IsUnicode(false);

            entity.HasOne(d => d.WorkLocation).WithMany(p => p.Users)
                .HasForeignKey(d => d.WorkLocationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_WorkLocation");
        });

        modelBuilder.Entity<UserClaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserClai__3214EC076EA7E35D");

            entity.HasIndex(e => e.UserId, "IX_UserClaims_UserId");

            entity.Property(e => e.ClaimType)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.UserClaims)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserClaims_User");
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__UserProf__1788CC4C2238CF9B");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.AvatarUrl).HasMaxLength(1000);
            entity.Property(e => e.CitizenId)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CitizenIdIssuedPlace).HasMaxLength(255);
            entity.Property(e => e.DisplayName)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.FullName)
                .HasMaxLength(255)
                .HasDefaultValue("");
            entity.Property(e => e.Gender)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.HometownAddress).HasMaxLength(500);
            entity.Property(e => e.HometownProvince).HasMaxLength(100);
            entity.Property(e => e.HometownWard).HasMaxLength(100);
            entity.Property(e => e.IdentityDocumentType)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MiddleName).HasMaxLength(100);
            entity.Property(e => e.NationalityCode)
                .HasMaxLength(2)
                .IsUnicode(false);
            entity.Property(e => e.PermanentAddress).HasMaxLength(500);
            entity.Property(e => e.PermanentProvince).HasMaxLength(100);
            entity.Property(e => e.PermanentWard).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(15)
                .IsUnicode(false)
                .HasDefaultValue("");
            entity.Property(e => e.PlaceOfBirth).HasMaxLength(255);
            entity.Property(e => e.PlaceOfBirthAddress).HasMaxLength(500);
            entity.Property(e => e.PlaceOfBirthProvince).HasMaxLength(100);
            entity.Property(e => e.PlaceOfBirthWard).HasMaxLength(100);
            entity.Property(e => e.VerificationStatus)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithOne(p => p.UserProfile)
                .HasForeignKey<UserProfile>(d => d.UserId)
                .HasConstraintName("FK_UserProfiles_Users");
        });

        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId });

            entity.HasIndex(e => e.RoleId, "IX_UserRoles_RoleId");

            entity.HasIndex(e => e.UserId, "IX_UserRoles_UserId");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Role).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK_UserRoles_Role");

            entity.HasOne(d => d.User).WithMany(p => p.UserRoles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_UserRoles_User");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3214EC07FC5BB95A");

            entity.HasIndex(e => e.Code, "IX_Warehouses_Code");

            entity.HasIndex(e => new { e.IsActive, e.IsDelete }, "IX_Warehouses_IsActive_IsDelete");

            entity.HasIndex(e => e.WorkLocationId, "IX_Warehouses_WorkLocationId");

            entity.HasIndex(e => e.Code, "UQ__Warehous__A25C5AA77CD1728C").IsUnique();

            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(300);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.WorkLocation).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.WorkLocationId)
                .HasConstraintName("FK_Warehouses_Location");
        });

        modelBuilder.Entity<WorkLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkLoca__3214EC07B741AD20");

            entity.HasIndex(e => e.Code, "IX_WorkLocations_Code");

            entity.HasIndex(e => new { e.IsActive, e.IsDelete }, "IX_WorkLocations_IsActive_IsDelete");

            entity.HasIndex(e => e.ManagerUserId, "IX_WorkLocations_ManagerUserId");

            entity.HasIndex(e => e.ProvinceName, "IX_WorkLocations_ProvinceName");

            entity.HasIndex(e => e.Code, "UQ__WorkLoca__A25C5AA7A73BB5A8").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(300);
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DistrictName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsDelete).HasDefaultValue(false);
            entity.Property(e => e.LocationType)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValue("Store");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ProvinceName).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

            entity.HasOne(d => d.ManagerUser).WithMany(p => p.WorkLocations)
                .HasForeignKey(d => d.ManagerUserId)
                .HasConstraintName("FK_WorkLocations_Manager");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
