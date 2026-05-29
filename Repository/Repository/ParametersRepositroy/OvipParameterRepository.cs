using Microsoft.EntityFrameworkCore;
using Models.Entities.Parameters;
using Repository.Context;
using Repository.Repository.ParametersRepositroy.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Repository.ParametersRepositroy
{
    public class OvipParameterRepository : IOvipParameterRepository
    {
        private readonly OvipDbContext _context;

        public OvipParameterRepository(OvipDbContext context)
        {
            _context = context;
        }

        // =========================
        // Összes paraméter
        // =========================
        public async Task<List<OvipParameter>> GetAllAsync()
        {
            return await _context.Parameters
                .ToListAsync();
        }

        // =========================
        // ID alapján
        // =========================
        public async Task<OvipParameter?> GetByIdAsync(int id)
        {
            return await _context.Parameters
                .FirstOrDefaultAsync(x => x.OvipParameterId == id);
        }

        // =========================
        // Létrehozás
        // =========================
        public async Task<OvipParameter> CreateAsync(OvipParameter entity)
        {
            await _context.Parameters.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        // =========================
        // Módosítás
        // =========================
        public async Task<OvipParameter?> UpdateAsync(OvipParameter entity)
        {
            var existing = await _context.Parameters
                .FirstOrDefaultAsync(x => x.OvipParameterId == entity.OvipParameterId);

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
            var entity = await _context.Parameters
                .FirstOrDefaultAsync(x => x.OvipParameterId == id);

            if (entity == null)
                return false;

            _context.Parameters.Remove(entity);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}