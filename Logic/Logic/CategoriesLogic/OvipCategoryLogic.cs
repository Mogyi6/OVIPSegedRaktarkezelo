using AutoMapper;
using Logic.Logic.CategoriesLogic.Interfaces;
using Models.Dtos.Categories;
using Models.Entities.Categories;
using Repository.Repository.CategoriesRepository.Interfaces;

namespace Logic.Logic.CategoriesLogic
{
    public class OvipCategoryLogic : IOvipCategoryLogic
    {
        private readonly IOvipCategoryRepository _repo;
        private readonly IMapper _mapper;

        public OvipCategoryLogic(IOvipCategoryRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<List<OvipCategory>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<OvipCategory?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipCategory> CreateAsync(OvipCategoryCreateDto dto)
        {
            var entity = _mapper.Map<OvipCategory>(dto);
            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipCategory?> UpdateAsync(OvipCategoryUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.OvipCategoryId);

            if (existing == null)
                return null;

            _mapper.Map(dto, existing);

            return await _repo.UpdateAsync(existing);
        }

        // =========================
        // DELETE
        // =========================
        public Task<bool> DeleteAsync(int id)
            => _repo.DeleteAsync(id);
    }
}