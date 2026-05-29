using AutoMapper;
using Logic.Logic.CategoriesLogic.Interfaces;
using Models.Dtos.Categories;
using Models.Entities.Categories;
using Repository.Repository.CategoriesRepository.Interfaces;

namespace Logic.Logic.CategoriesLogic
{
    public class OvipCategoryConnectionLogic : IOvipCategoryConnectionLogic
    {
        private readonly IOvipCategoryConnectionRepository _repo;
        private readonly IMapper _mapper;

        public OvipCategoryConnectionLogic(
            IOvipCategoryConnectionRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<List<OvipCategoryConnection>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<OvipCategoryConnection?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // CREATE
        public async Task<OvipCategoryConnection> CreateAsync(OvipCategoryConnectionCreateDto dto)
        {
            var entity = _mapper.Map<OvipCategoryConnection>(dto);
            return await _repo.CreateAsync(entity);
        }

        // UPDATE
        public async Task<OvipCategoryConnection?> UpdateAsync(OvipCategoryConnectionUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);

            if (existing == null)
                return null;

            _mapper.Map(dto, existing);

            return await _repo.UpdateAsync(existing);
        }

        public Task<bool> DeleteAsync(int id)
            => _repo.DeleteAsync(id);
    }
}