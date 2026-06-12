using Microsoft.EntityFrameworkCore;
using Models.Entities;

namespace Repository.Context
{
    public class OvipDbContext : DbContext
    {
        public OvipDbContext(DbContextOptions<OvipDbContext> options)
            : base(options)
        {
        }


        // =========================
        public DbSet<Category> Categories { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           

            // =========================================================
            // CATEGORY
            // =========================================================
            modelBuilder.Entity<Category>(entity =>
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

        }
    }
}