using AutoMapper;
using Logic.Logic.PricingLogic.Interfaces;
using Models.Dtos.Pircing;
using Models.Entities.Pricing;
using Repository.Repository.PricingRepository.Interfaces;

namespace Logic.Logic.PricingLogic
{
    public class OvipQuantityDiscountLogic : IOvipQuantityDiscountLogic
    {
        private readonly IOvipQuantityDiscountRepository _repo;
        private readonly IMapper _mapper;

        public OvipQuantityDiscountLogic(
            IOvipQuantityDiscountRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public Task<List<OvipQuantityDiscount>> GetAllAsync()
            => _repo.GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        public Task<OvipQuantityDiscount?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // BY PRODUCT
        // =========================
        public Task<List<OvipQuantityDiscount>> GetByProductIdAsync(int productId)
            => _repo.GetByProductIdAsync(productId);

        // =========================
        // BY PRICE LIST
        // =========================
        public Task<List<OvipQuantityDiscount>> GetByPriceListIdAsync(int priceListId)
            => _repo.GetByPriceListIdAsync(priceListId);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipQuantityDiscount> CreateAsync(OvipQuantityDiscountCreateDto dto)
        {
            var entity = _mapper.Map<OvipQuantityDiscount>(dto);
            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipQuantityDiscount?> UpdateAsync(OvipQuantityDiscountUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.OvipQuantityId);

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