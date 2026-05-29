using Microsoft.EntityFrameworkCore;
using Models.Entities.Categories;
using Repository.Context;
using Repository.Repository.CategoriesRepository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.CategoriesRepository
{
    public class OvipCategoryConnectionRepository : IOvipCategoryConnectionRepository
    {
        private readonly OvipDbContext _context;

        public OvipCategoryConnectionRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes kapcsolat
        // =========================
        public async Task<List<OvipCategoryConnection>> GetAllAsync()
        {
            return await _context.CategoryConnections
                .Include(x => x.Product)
                .Include(x => x.Category)
                .ToListAsync();
        }

        // =========================
        // ID alapján
        // =========================
        public async Task<OvipCategoryConnection?> GetByIdAsync(int id)
        {
            return await _context.CategoryConnections
                .Include(x => x.Product)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // Termék alapján
        // =========================
        public async Task<List<OvipCategoryConnection>> GetByProductIdAsync(int productId)
        {
            return await _context.CategoryConnections
                .Include(x => x.Category)
                .Where(x => x.OvipProductId == productId)
                .ToListAsync();
        }

        // =========================
        // Kategória alapján
        // =========================
        public async Task<List<OvipCategoryConnection>> GetByCategoryIdAsync(int categoryId)
        {
            return await _context.CategoryConnections
                .Include(x => x.Product)
                .Where(x => x.OvipCategoryId == categoryId)
                .ToListAsync();
        }

        // =========================
        // Létrehozás
        // =========================
        public async Task<OvipCategoryConnection> CreateAsync(OvipCategoryConnection entity)
        {
            await _context.CategoryConnections.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // =========================
        // Módosítás
        // =========================
        public async Task<OvipCategoryConnection?> UpdateAsync(OvipCategoryConnection entity)
        {
            var existing = await _context.CategoryConnections
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
            var entity = await _context.CategoryConnections
                .FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                return false;

            _context.CategoryConnections.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}