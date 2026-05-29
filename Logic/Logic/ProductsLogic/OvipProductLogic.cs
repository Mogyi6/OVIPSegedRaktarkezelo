using AutoMapper;
using Logic.Logic.ProductsLogic.Interfaces;
using Models.Dtos.Products;
using Models.Entities.Products;
using Repository.Repository.ProductsRepository.Interfaces;

namespace Logic.Logic.ProductsLogic
{
    public class OvipProductLogic : IOvipProductLogic
    {
        private readonly IOvipProductRepository _repo;
        private readonly IMapper _mapper;

        public OvipProductLogic(IOvipProductRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<List<OvipProduct>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<OvipProduct?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public async Task<OvipProduct> CreateAsync(OvipProductCreateDto dto)
        {
            var entity = _mapper.Map<OvipProduct>(dto);

            return await _repo.CreateAsync(entity);
        }

        public async Task<OvipProduct?> UpdateAsync(OvipProductUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.OvipProductId);

            if (existing == null)
                return null;

            _mapper.Map(dto, existing);

            return await _repo.UpdateAsync(existing);
        }

        public Task<bool> DeleteAsync(int id)
            => _repo.DeleteAsync(id);
    }
}