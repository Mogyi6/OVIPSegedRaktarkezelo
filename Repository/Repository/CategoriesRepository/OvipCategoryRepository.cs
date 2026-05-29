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
    public class OvipCategoryRepository : IOvipCategoryRepository
    {
        private readonly OvipDbContext _context;

        public OvipCategoryRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes kategória
        // =========================
        public async Task<List<OvipCategory>> GetAllAsync()
        {
            return await _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.Children)
                .Include(x => x.Products)
                .ToListAsync();
        }

        // =========================
        // ID alapján
        // =========================
        public async Task<OvipCategory?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.Children)
                .Include(x => x.Products)
                .FirstOrDefaultAsync(x => x.OvipCategoryId == id);
        }

        // =========================
        // Létrehozás
        // =========================
        public async Task<OvipCategory> CreateAsync(OvipCategory entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // =========================
        // Módosítás
        // =========================
        public async Task<OvipCategory?> UpdateAsync(OvipCategory entity)
        {
            var existing = await _context.Categories
                .FirstOrDefaultAsync(x => x.OvipCategoryId == entity.OvipCategoryId);

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
            var entity = await _context.Categories
                .FirstOrDefaultAsync(x => x.OvipCategoryId == id);

            if (entity == null)
                return false;

            _context.Categories.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
