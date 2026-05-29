using Microsoft.EntityFrameworkCore;
using Models.Entities.Products;
using Repository.Context;
using Repository.Repository.ProductsRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.ProductsRepository
{
    public class OvipStockRepository : IOvipStockRepository
    {
        private readonly OvipDbContext _context;

        public OvipStockRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes készlet lekérése
        // =========================
        public async Task<List<OvipStock>> GetAllAsync()
        {
            return await _context.Stocks
                .Include(x => x.Product)
                .ToListAsync();
        }

        // =========================
        // Készlet lekérése ID alapján
        // =========================
        public async Task<OvipStock?> GetByIdAsync(int id)
        {
            return await _context.Stocks
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // Termék készlet lekérése
        // =========================
        public async Task<OvipStock?> GetByProductIdAsync(int productId)
        {
            return await _context.Stocks
                .FirstOrDefaultAsync(x => x.OvipProductId == productId);
        }

        // =========================
        // Új készlet létrehozása
        // =========================
        public async Task<OvipStock> CreateAsync(OvipStock stock)
        {
            stock.UpdatedAt = DateTime.UtcNow;

            await _context.Stocks.AddAsync(stock);
            await _context.SaveChangesAsync();

            return stock;
        }

        // =========================
        // Készlet módosítása
        // =========================
        public async Task<OvipStock?> UpdateAsync(OvipStock stock)
        {
            var existingStock = await _context.Stocks
                .FirstOrDefaultAsync(x => x.Id == stock.Id);

            if (existingStock == null)
                return null;

            _context.Entry(existingStock)
                .CurrentValues
                .SetValues(stock);

            existingStock.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return existingStock;
        }

        // =========================
        // Készlet törlése
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var stock = await _context.Stocks
                .FirstOrDefaultAsync(x => x.Id == id);

            if (stock == null)
                return false;

            _context.Stocks.Remove(stock);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}