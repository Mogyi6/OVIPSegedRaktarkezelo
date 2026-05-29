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
    public class OvipPriceListPriceRepository : IOvipPriceListPriceRepository
    {
        private readonly OvipDbContext _context;

        public OvipPriceListPriceRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes árlista ár
        // =========================
        public async Task<List<OvipPriceListPrice>> GetAllAsync()
        {
            return await _context.PriceListPrices
                .Include(x => x.PriceList)
                .Include(x => x.Product)
                .ToListAsync();
        }

        // =========================
        // ID alapján
        // =========================
        public async Task<OvipPriceListPrice?> GetByIdAsync(int id)
        {
            return await _context.PriceListPrices
                .Include(x => x.PriceList)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // Árlista alapján
        // =========================
        public async Task<List<OvipPriceListPrice>> GetByPriceListIdAsync(int priceListId)
        {
            return await _context.PriceListPrices
                .Include(x => x.Product)
                .Where(x => x.OvipPriceListId == priceListId)
                .ToListAsync();
        }

        // =========================
        // Termék alapján
        // =========================
        public async Task<List<OvipPriceListPrice>> GetByProductIdAsync(int productId)
        {
            return await _context.PriceListPrices
                .Include(x => x.PriceList)
                .Where(x => x.OvipProductId == productId)
                .ToListAsync();
        }

        // =========================
        // Létrehozás
        // =========================
        public async Task<OvipPriceListPrice> CreateAsync(OvipPriceListPrice entity)
        {
            await _context.PriceListPrices.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // =========================
        // Módosítás
        // =========================
        public async Task<OvipPriceListPrice?> UpdateAsync(OvipPriceListPrice entity)
        {
            var existing = await _context.PriceListPrices
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

            if (existing == null)
                return null;

            // automatikus mezőmásolás (NEM kézzel)
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
            var entity = await _context.PriceListPrices
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.PriceListPrices.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}