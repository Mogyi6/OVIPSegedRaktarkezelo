using Microsoft.EntityFrameworkCore;
using Models.Entities.Categories;
using Models.Entities.Manufacture;
using Models.Entities.Parameters;
using Models.Entities.Pricing;
using Models.Entities.Products;

namespace Repository.Context
{
    public class OvipDbContext : DbContext
    {
        public OvipDbContext(DbContextOptions<OvipDbContext> options)
            : base(options)
        {
        }

        // =========================
        // PRODUCTS
        // =========================
        public DbSet<OvipProduct> Products { get; set; }
        public DbSet<OvipProductImage> ProductImages { get; set; }
        public DbSet<OvipProductParameter> ProductParameters { get; set; }
        public DbSet<OvipStock> Stocks { get; set; }

        // =========================
        // CATEGORIES
        // =========================
        public DbSet<OvipCategory> Categories { get; set; }
        public DbSet<OvipCategoryConnection> CategoryConnections { get; set; }

        // =========================
        // PARAMETERS
        // =========================
        public DbSet<OvipParameter> Parameters { get; set; }

        // =========================
        // PRICING
        // =========================
        public DbSet<OvipPriceList> PriceLists { get; set; }
        public DbSet<OvipPriceListPrice> PriceListPrices { get; set; }
        public DbSet<OvipQuantityDiscount> QuantityDiscounts { get; set; }

        // =========================
        // MANUFACTURE
        // =========================
        public DbSet<OvipManufacture> Manufactures { get; set; }
        public DbSet<OvipManufacturePart> ManufactureParts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // =========================================================
            // PRODUCT
            // =========================================================
            modelBuilder.Entity<OvipProduct>(entity =>
            {
                entity.ToTable("Products");

                entity.HasKey(x => x.OvipProductId);

                entity.Property(x => x.OvipProductId)
                    .ValueGeneratedNever();

                entity.Property(x => x.Name)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(x => x.Sku).HasMaxLength(255);
                entity.Property(x => x.Barcode).HasMaxLength(255);
                entity.Property(x => x.Manufacturer).HasMaxLength(255);
                entity.Property(x => x.Unit).HasMaxLength(50);
                entity.Property(x => x.AltUnit).HasMaxLength(50);
                entity.Property(x => x.SeoTitle).HasMaxLength(500);
                entity.Property(x => x.SeoDescription).HasMaxLength(2000);

                entity.Property(x => x.ShortDescription).HasColumnType("longtext");
                entity.Property(x => x.LongDescription).HasColumnType("longtext");

                entity.HasOne(x => x.MainCategory)
                    .WithMany()
                    .HasForeignKey(x => x.OvipCategoryId)
                    .OnDelete(DeleteBehavior.NoAction);

                entity.HasOne(x => x.VariantProduct)
                    .WithMany()
                    .HasForeignKey(x => x.ProductVariantId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // =========================================================
            // CATEGORY
            // =========================================================
            modelBuilder.Entity<OvipCategory>(entity =>
            {
                entity.ToTable("Categories");

                entity.HasKey(x => x.OvipCategoryId);

                entity.Property(x => x.OvipCategoryId)
                    .ValueGeneratedNever();

                entity.Property(x => x.Name)
                    .HasMaxLength(500)
                    .IsRequired();

                entity.Property(x => x.Description)
                    .HasColumnType("longtext");

                entity.HasOne(x => x.ParentCategory)
                    .WithMany(x => x.Children)
                    .HasForeignKey(x => x.ParentCategoryId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // =========================================================
            // CATEGORY CONNECTION
            // =========================================================
            modelBuilder.Entity<OvipCategoryConnection>(entity =>
            {
                entity.ToTable("CategoryConnections");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.HasIndex(x => new
                {
                    x.OvipProductId,
                    x.OvipCategoryId
                }).IsUnique();

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.Categories)
                    .HasForeignKey(x => x.OvipProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Category)
                    .WithMany(x => x.Products)
                    .HasForeignKey(x => x.OvipCategoryId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================================================
            // PARAMETER
            // =========================================================
            modelBuilder.Entity<OvipParameter>(entity =>
            {
                entity.ToTable("Parameters");

                entity.HasKey(x => x.OvipParameterId);

                entity.Property(x => x.OvipParameterId)
                    .ValueGeneratedNever();

                entity.Property(x => x.ParameterName)
                    .HasMaxLength(500)
                    .IsRequired();
            });

            // =========================================================
            // PRODUCT PARAMETER
            // =========================================================
            modelBuilder.Entity<OvipProductParameter>(entity =>
            {
                entity.ToTable("ProductParameters");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.Property(x => x.ParameterValue)
                    .HasMaxLength(1000);

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.Parameters)
                    .HasForeignKey(x => x.OvipProductId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Parameter)
                    .WithMany()
                    .HasForeignKey(x => x.OvipParameterId)
                    .OnDelete(DeleteBehavior.NoAction);
            });

            // =========================================================
            // PRODUCT IMAGE
            // =========================================================
            modelBuilder.Entity<OvipProductImage>(entity =>
            {
                entity.ToTable("ProductImages");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.Property(x => x.Image)
                    .HasMaxLength(2000)
                    .IsRequired();

                entity.Property(x => x.ThumbnailImage)
                    .HasMaxLength(2000);

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.Images)
                    .HasForeignKey(x => x.OvipProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================================================
            // STOCK
            // =========================================================
            modelBuilder.Entity<OvipStock>(entity =>
            {
                entity.ToTable("Stocks");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.HasIndex(x => x.OvipProductId).IsUnique();

                entity.HasOne(x => x.Product)
                    .WithOne(x => x.Stock)
                    .HasForeignKey<OvipStock>(x => x.OvipProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================================================
            // PRICE LIST
            // =========================================================
            modelBuilder.Entity<OvipPriceList>(entity =>
            {
                entity.ToTable("PriceLists");

                entity.HasKey(x => x.OvipPriceListId);

                entity.Property(x => x.OvipPriceListId)
                    .ValueGeneratedNever();

                entity.Property(x => x.Name)
                    .HasMaxLength(500)
                    .IsRequired();
            });

            // =========================================================
            // PRICE LIST PRICE
            // =========================================================
            modelBuilder.Entity<OvipPriceListPrice>(entity =>
            {
                entity.ToTable("PriceListPrices");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.HasOne(x => x.PriceList)
                    .WithMany(x => x.Prices)
                    .HasForeignKey(x => x.OvipPriceListId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.Product)
                    .WithMany(x => x.PricelistPrices)
                    .HasForeignKey(x => x.OvipProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================================================
            // QUANTITY DISCOUNT
            // =========================================================
            modelBuilder.Entity<OvipQuantityDiscount>(entity =>
            {
                entity.ToTable("QuantityDiscounts");

                entity.HasKey(x => x.OvipQuantityId);

                entity.Property(x => x.OvipQuantityId)
                    .ValueGeneratedNever();

                entity.Property(x => x.DiscountType)
                    .HasMaxLength(100);
            });

            // =========================================================
            // MANUFACTURE
            // =========================================================
            modelBuilder.Entity<OvipManufacture>(entity =>
            {
                entity.ToTable("Manufactures");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.HasIndex(x => x.OvipProductId).IsUnique();

                entity.HasOne(x => x.Product)
                    .WithOne(x => x.Manufacture)
                    .HasForeignKey<OvipManufacture>(x => x.OvipProductId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // =========================================================
            // MANUFACTURE PART
            // =========================================================
            modelBuilder.Entity<OvipManufacturePart>(entity =>
            {
                entity.ToTable("ManufactureParts");

                entity.HasKey(x => x.Id);

                entity.Property(x => x.Id)
                    .ValueGeneratedNever();

                entity.HasOne(x => x.Manufacture)
                    .WithMany(x => x.Parts)
                    .HasForeignKey(x => x.ManufactureId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(x => x.PartProduct)
                    .WithMany()
                    .HasForeignKey(x => x.PartProductId)
                    .OnDelete(DeleteBehavior.NoAction);
            });
        }
    }
}