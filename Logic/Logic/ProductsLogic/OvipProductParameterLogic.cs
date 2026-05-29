using AutoMapper;
using Logic.Logic.ProductsLogic.Interfaces;
using Models.Dtos.Products;
using Models.Entities.Products;
using Repository.Repository.ProductsRepository.Interfaces;

namespace Logic.Logic.ProductsLogic
{
    public class OvipProductParameterLogic : IOvipProductParameterLogic
    {
        private readonly IOvipProductParameterRepository _repo;
        private readonly IMapper _mapper;

        public OvipProductParameterLogic(
            IOvipProductParameterRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public Task<List<OvipProductParameter>> GetAllAsync()
            => _repo.GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        public Task<OvipProductParameter?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // GET BY PRODUCT ID
        // =========================
        public Task<List<OvipProductParameter>> GetByProductIdAsync(int productId)
            => _repo.GetByProductIdAsync(productId);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipProductParameter> CreateAsync(OvipProductParameterCreateDto dto)
        {
            var entity = _mapper.Map<OvipProductParameter>(dto);

            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipProductParameter?> UpdateAsync(OvipProductParameterUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);

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