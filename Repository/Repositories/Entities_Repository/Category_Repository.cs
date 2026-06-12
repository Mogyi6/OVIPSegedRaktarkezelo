using Microsoft.EntityFrameworkCore;
using Models.Entities;
using Repository.Context;
using Repository.Repositories.Entities_Repository.Entities_Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repositories.Entities_Repository
{
    public class Category_Repository : ICategory_Repository
    {
        private readonly OvipDbContext _context;

        public Category_Repository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // CREATE
        // =========================
        public async Task<Category> CreateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();

            return category;
        }

        // =========================
        // READ (ALL)
        // =========================
        public async Task<List<Category>> GetAllAsync()
        {
            return await _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.Children)
                .ToListAsync();
        }

        // =========================
        // READ (BY ID)
        // =========================
        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.Children)
                .FirstOrDefaultAsync(x => x.OvipCategoryId == id);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<bool> UpdateAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category));

            var existing = await _context.Categories
                .FirstOrDefaultAsync(x => x.OvipCategoryId == category.OvipCategoryId);

            if (existing == null)
                return false;

            existing.Name = category.Name;
            existing.Description = category.Description;
            existing.SeoTitle = category.SeoTitle;
            existing.SeoDescription = category.SeoDescription;
            existing.Image = category.Image;
            existing.Order = category.Order;
            existing.ParentCategoryId = category.ParentCategoryId;

            await _context.SaveChangesAsync();

            return true;
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories
                .FirstOrDefaultAsync(x => x.OvipCategoryId == id);

            if (category == null)
                return false;

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}

