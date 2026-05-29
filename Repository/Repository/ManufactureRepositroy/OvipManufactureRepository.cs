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
    public class OvipManufactureRepository : IOvipManufactureRepository
    {
        private readonly OvipDbContext _context;

        public OvipManufactureRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes gyártás
        // =========================
        public async Task<List<OvipManufacture>> GetAllAsync()
        {
            return await _context.Manufactures
                .Include(x => x.Product)
                .Include(x => x.Parts)
                .ToListAsync();
        }

        // =========================
        // ID alapján
        // =========================
        public async Task<OvipManufacture?> GetByIdAsync(int id)
        {
            return await _context.Manufactures
                .Include(x => x.Product)
                .Include(x => x.Parts)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // Termék alapján
        // =========================
        public async Task<OvipManufacture?> GetByProductIdAsync(int productId)
        {
            return await _context.Manufactures
                .Include(x => x.Parts)
                .FirstOrDefaultAsync(x => x.OvipProductId == productId);
        }

        // =========================
        // Létrehozás
        // =========================
        public async Task<OvipManufacture> CreateAsync(OvipManufacture entity)
        {
            await _context.Manufactures.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // =========================
        // Módosítás
        // =========================
        public async Task<OvipManufacture?> UpdateAsync(OvipManufacture entity)
        {
            var existing = await _context.Manufactures
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
            var entity = await _context.Manufactures
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.Manufactures.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}