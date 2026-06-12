using Logic.Logics.Entities_Logic.Entities_Logic_Interfaces;
using Models.Entities;
using Repository.Repositories.Entities_Repository.Entities_Repository_Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Logics.Entities_Logic
{
    public class Category_Logic : ICategory_Logic
    {
        private readonly ICategory_Repository _categoryRepository;

        public Category_Logic(ICategory_Repository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // =========================
        // CREATE
        // =========================
        public async Task<Category?> CreateAsync(Category category)
        {
            try
            {
                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                return await _categoryRepository.CreateAsync(category);
            }
            catch (Exception ex)
            {
                // ide jöhet logger is
                throw new Exception("Hiba történt a kategória létrehozásakor.", ex);
            }
        }

        // =========================
        // GET ALL
        // =========================
        public async Task<List<Category>?> GetAllAsync()
        {
            try
            {
                return await _categoryRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Hiba történt a kategóriák lekérésekor.", ex);
            }
        }

        // =========================
        // GET BY ID
        // =========================
        public async Task<Category?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Érvénytelen ID.");

                return await _categoryRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Hiba történt a kategória lekérésekor.", ex);
            }
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                if (category == null)
                    throw new ArgumentNullException(nameof(category));

                return await _categoryRepository.UpdateAsync(category);
            }
            catch (Exception ex)
            {
                throw new Exception("Hiba történt a kategória módosításakor.", ex);
            }
        }

        // =========================
        // DELETE
        // =========================
        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Érvénytelen ID.");

                return await _categoryRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Hiba történt a kategória törlésekor.", ex);
            }
        }
    }
}

