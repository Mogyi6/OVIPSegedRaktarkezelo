using Microsoft.EntityFrameworkCore;
using Models.Entities.Pricing;
using Repository.Context;
using Repository.Repository.PricingRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.PricingRepository
{
    public class OvipQuantityDiscountRepository : IOvipQuantityDiscountRepository
    {
        private readonly OvipDbContext _context;

        public OvipQuantityDiscountRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes mennyiségi kedvezmény
        // =========================
        public async Task<List<OvipQuantityDiscount>> GetAllAsync()
        {
            return await _context.QuantityDiscounts
                .ToListAsync();
        }

        // =========================
        // ID alapján
        // =========================
        public async Task<OvipQuantityDiscount?> GetByIdAsync(int id)
        {
            return await _context.QuantityDiscounts
                .FirstOrDefaultAsync(x => x.OvipQuantityId == id);
        }

        // =========================
        // Termék alapján
        // =========================
        public async Task<List<OvipQuantityDiscount>> GetByProductIdAsync(int productId)
        {
            return await _context.QuantityDiscounts
                .Where(x => x.ProductId == productId)
                .ToListAsync();
        }

        // =========================
        // Árlista alapján
        // =========================
        public async Task<List<OvipQuantityDiscount>> GetByPriceListIdAsync(int priceListId)
        {
            return await _context.QuantityDiscounts
                .Where(x => x.PriceListId == priceListId)
                .ToListAsync();
        }

        // =========================
        // Létrehozás
        // =========================
        public async Task<OvipQuantityDiscount> CreateAsync(OvipQuantityDiscount entity)
        {
            await _context.QuantityDiscounts.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // =========================
        // Módosítás
        // =========================
        public async Task<OvipQuantityDiscount?> UpdateAsync(OvipQuantityDiscount entity)
        {
            var existing = await _context.QuantityDiscounts
                .FirstOrDefaultAsync(x => x.OvipQuantityId == entity.OvipQuantityId);

            if (existing == null)
                return null;

            _context.Entry(existing)
                .CurrentValues
                .SetValues(entity);

            await _context.SaveChangesAsync();

            return existing;
        }

        // =========================
        // Törlés
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.QuantityDiscounts
                .FirstOrDefaultAsync(x => x.OvipQuantityId == id);

            if (entity == null)
                return false;

            _context.QuantityDiscounts.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}