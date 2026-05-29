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
    public class OvipProductImageRepository : IOvipProductImageRepository
    {
        private readonly OvipDbContext _context;

        public OvipProductImageRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes kép lekérése
        // =========================
        public async Task<List<OvipProductImage>> GetAllAsync()
        {
            return await _context.ProductImages
                .Include(x => x.Product)
                .ToListAsync();
        }

        // =========================
        // Kép lekérése ID alapján
        // =========================
        public async Task<OvipProductImage?> GetByIdAsync(int id)
        {
            return await _context.ProductImages
                .Include(x => x.Product)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // Termék képeinek lekérése
        // =========================
        public async Task<List<OvipProductImage>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductImages
                .Where(x => x.OvipProductId == productId)
                .OrderBy(x => x.SortOrder)
                .ToListAsync();
        }

        // =========================
        // Új kép létrehozása
        // =========================
        public async Task<OvipProductImage> CreateAsync(OvipProductImage image)
        {
            await _context.ProductImages.AddAsync(image);

            await _context.SaveChangesAsync();

            return image;
        }

        // =========================
        // Kép módosítása
        // =========================
        public async Task<OvipProductImage?> UpdateAsync(OvipProductImage image)
        {
            var existingImage = await _context.ProductImages
                .FirstOrDefaultAsync(x => x.Id == image.Id);

            if (existingImage == null)
                return null;

            _context.Entry(existingImage).CurrentValues.SetValues(image);

            await _context.SaveChangesAsync();

            return existingImage;
        }

        // =========================
        // Kép törlése
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var image = await _context.ProductImages
                .FirstOrDefaultAsync(x => x.Id == id);

            if (image == null)
                return false;

            _context.ProductImages.Remove(image);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}