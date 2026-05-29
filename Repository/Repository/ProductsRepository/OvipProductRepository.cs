using Microsoft.EntityFrameworkCore;
using Models.Entities.Products;
using Repository.Context;
using Repository.Repository.ProductsRepository.Interfaces;

namespace Repository.Repository.ProductsRepository
{
    public class OvipProductRepository : IOvipProductRepository
    {
        private readonly OvipDbContext _context;

        public OvipProductRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<OvipProduct>> GetAllAsync()
        {
            return await _context.Products
                .Include(x => x.MainCategory)
                .Include(x => x.Categories)
                .Include(x => x.Parameters)
                .Include(x => x.Images)
                .Include(x => x.Stock)
                .Include(x => x.QuantityDiscounts)
                .Include(x => x.PricelistPrices)
                .Include(x => x.Manufacture)
                .ToListAsync();
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<OvipProduct?> GetByIdAsync(int id)
        {
            return await _context.Products
                .Include(x => x.MainCategory)
                .Include(x => x.Categories)
                .Include(x => x.Parameters)
                .Include(x => x.Images)
                .Include(x => x.Stock)
                .Include(x => x.QuantityDiscounts)
                .Include(x => x.PricelistPrices)
                .Include(x => x.Manufacture)
                .FirstOrDefaultAsync(x => x.OvipProductId == id);
        }

        // =========================
        // CREATE (ENTITY!)
        // =========================
        public async Task<OvipProduct> CreateAsync(OvipProduct product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        // =========================
        // UPDATE (ENTITY!)
        // =========================
        public async Task<OvipProduct?> UpdateAsync(OvipProduct product)
        {
            var existing = await _context.Products
                .FirstOrDefaultAsync(x => x.OvipProductId == product.OvipProductId);

            if (existing == null)
                return null;

            _context.Entry(existing).CurrentValues.SetValues(product);

            await _context.SaveChangesAsync();

            return existing;
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return false;

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}