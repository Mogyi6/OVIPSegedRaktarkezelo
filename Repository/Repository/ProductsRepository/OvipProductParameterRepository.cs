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
    public class OvipProductParameterRepository : IOvipProductParameterRepository
    {
        private readonly OvipDbContext _context;

        public OvipProductParameterRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes paraméter lekérése
        // =========================
        public async Task<List<OvipProductParameter>> GetAllAsync()
        {
            return await _context.ProductParameters
                .Include(x => x.Product)
                .Include(x => x.Parameter)
                .ToListAsync();
        }

        // =========================
        // Paraméter lekérése ID alapján
        // =========================
        public async Task<OvipProductParameter?> GetByIdAsync(int id)
        {
            return await _context.ProductParameters
                .Include(x => x.Product)
                .Include(x => x.Parameter)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        // =========================
        // Termék paramétereinek lekérése
        // =========================
        public async Task<List<OvipProductParameter>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductParameters
                .Include(x => x.Parameter)
                .Where(x => x.OvipProductId == productId)
                .ToListAsync();
        }

        // =========================
        // Új paraméter létrehozása
        // =========================
        public async Task<OvipProductParameter> CreateAsync(OvipProductParameter parameter)
        {
            await _context.ProductParameters.AddAsync(parameter);

            await _context.SaveChangesAsync();

            return parameter;
        }

        // =========================
        // Paraméter módosítása
        // =========================
        public async Task<OvipProductParameter?> UpdateAsync(OvipProductParameter parameter)
        {
            var existingParameter = await _context.ProductParameters
                .FirstOrDefaultAsync(x => x.Id == parameter.Id);

            if (existingParameter == null)
                return null;

            _context.Entry(existingParameter)
                .CurrentValues
                .SetValues(parameter);

            await _context.SaveChangesAsync();

            return existingParameter;
        }

        // =========================
        // Paraméter törlése
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            var parameter = await _context.ProductParameters
                .FirstOrDefaultAsync(x => x.Id == id);

            if (parameter == null)
                return false;

            _context.ProductParameters.Remove(parameter);

            await _context.SaveChangesAsync();

            return true;
        }
    }
}