using Microsoft.EntityFrameworkCore;
using Models.Entities.Manufacture;
using Repository.Context;
using Repository.Repository.ManufactureRepositroy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.ManufactureRepositroy
{
    public class OvipManufacturePartRepository : IOvipManufacturePartRepository
    {
        private readonly OvipDbContext _context;

        public OvipManufacturePartRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes gyártási alkatrész
        // =========================
        public async Task<List<OvipManufacturePart>> GetAllAsync()
        {
            return await _context.ManufactureParts
                .Include(x => x.Manufacture)
                .Include(x => x.PartProduct)
                .ToListAsync();
        }

        // =========================
        // ID alapján
        // =========================
        public async Task<OvipManufacturePart?> GetByIdAsync(int id)
        {
            return await _context.ManufactureParts
                .Include(x => x.Manufacture)
                .Include(x => x.PartProduct)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // Gyártás alapján
        // =========================
        public async Task<List<OvipManufacturePart>> GetByManufactureIdAsync(int manufactureId)
        {
            return await _context.ManufactureParts
                .Include(x => x.PartProduct)
                .Where(x => x.ManufactureId == manufactureId)
                .ToListAsync();
        }

        // =========================
        // Alkatrész termék alapján
        // =========================
        public async Task<List<OvipManufacturePart>> GetByPartProductIdAsync(int productId)
        {
            return await _context.ManufactureParts
                .Include(x => x.Manufacture)
                .Where(x => x.PartProductId == productId)
                .ToListAsync();
        }

        // =========================
        // Létrehozás
        // =========================
        public async Task<OvipManufacturePart> CreateAsync(OvipManufacturePart entity)
        {
            await _context.ManufactureParts.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // =========================
        // Módosítás
        // =========================
        public async Task<OvipManufacturePart?> UpdateAsync(OvipManufacturePart entity)
        {
            var existing = await _context.ManufactureParts
                .FirstOrDefaultAsync(x => x.Id == entity.Id);

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
            var entity = await _context.ManufactureParts
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.ManufactureParts.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}