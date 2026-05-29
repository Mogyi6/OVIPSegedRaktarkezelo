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
    public class OvipPriceListRepository : IOvipPriceListRepository
    {
        private readonly OvipDbContext _context;

        public OvipPriceListRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes árlista
        // =========================
        public async Task<List<OvipPriceList>> GetAllAsync()
        {
            return await _context.PriceLists
                .Include(x => x.Prices)
                .ToListAsync();
        }

        // =========================
        // Árlista ID alapján
        // =========================
        public async Task<OvipPriceList?> GetByIdAsync(int id)
        {
            return await _context.PriceLists
                .Include(x => x.Prices)
                .FirstOrDefaultAsync(x => x.OvipPriceListId == id);
        }

        // =========================
        // Új árlista
        // =========================
        public async Task<OvipPriceList> CreateAsync(OvipPriceList priceList)
        {
            await _context.PriceLists.AddAsync(priceList);
            await _context.SaveChangesAsync();
            return priceList;
        }

        // =========================
        // Árlista módosítása
        // =========================
        public async Task<OvipPriceList?> UpdateAsync(OvipPriceList priceList)
        {
            var existing = await _context.PriceLists
                .FirstOrDefaultAsync(x => x.OvipPriceListId == priceList.OvipPriceListId);

            if (existing == null)
                return null;

            _context.Entry(existing)
                .CurrentValues
                .SetValues(priceList);

            await _context.SaveChangesAsync();

            return existing;
        }

        // =========================
        // Árlista törlése
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.PriceLists
                .FirstOrDefaultAsync(x => x.OvipPriceListId == id);

            if (entity == null)
                return false;

            _context.PriceLists.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}