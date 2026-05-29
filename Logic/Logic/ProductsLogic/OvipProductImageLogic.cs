using AutoMapper;
using Logic.Logic.ProductsLogic.Interfaces;
using Models.Dtos.Products;
using Models.Entities.Products;
using Repository.Repository.ProductsRepository.Interfaces;

namespace Logic.Logic.ProductsLogic
{
    public class OvipProductImageLogic : IOvipProductImageLogic
    {
        private readonly IOvipProductImageRepository _repo;
        private readonly IMapper _mapper;

        public OvipProductImageLogic(
            IOvipProductImageRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public Task<List<OvipProductImage>> GetAllAsync()
            => _repo.GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        public Task<OvipProductImage?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // GET BY PRODUCT ID
        // =========================
        public Task<List<OvipProductImage>> GetByProductIdAsync(int productId)
            => _repo.GetByProductIdAsync(productId);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipProductImage> CreateAsync(OvipProductImageCreateDto dto)
        {
            var entity = _mapper.Map<OvipProductImage>(dto);

            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipProductImage?> UpdateAsync(OvipProductImageUpdateDto dto)
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