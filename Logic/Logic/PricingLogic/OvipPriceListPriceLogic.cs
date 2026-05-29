using AutoMapper;
using Logic.Logic.PricingLogic.Interfaces;
using Models.Dtos.Pircing;
using Models.Entities.Pricing;
using Repository.Repository.PricingRepository.Interfaces;

namespace Logic.Logic.PricingLogic
{
    public class OvipPriceListPriceLogic : IOvipPriceListPriceLogic
    {
        private readonly IOvipPriceListPriceRepository _repo;
        private readonly IMapper _mapper;

        public OvipPriceListPriceLogic(
            IOvipPriceListPriceRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public Task<List<OvipPriceListPrice>> GetAllAsync()
            => _repo.GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        public Task<OvipPriceListPrice?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // BY PRICE LIST
        // =========================
        public Task<List<OvipPriceListPrice>> GetByPriceListIdAsync(int priceListId)
            => _repo.GetByPriceListIdAsync(priceListId);

        // =========================
        // BY PRODUCT
        // =========================
        public Task<List<OvipPriceListPrice>> GetByProductIdAsync(int productId)
            => _repo.GetByProductIdAsync(productId);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipPriceListPrice> CreateAsync(OvipPriceListPriceCreateDto dto)
        {
            var entity = _mapper.Map<OvipPriceListPrice>(dto);
            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipPriceListPrice?> UpdateAsync(OvipPriceListPriceUpdateDto dto)
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