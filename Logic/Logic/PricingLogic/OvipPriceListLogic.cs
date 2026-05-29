using AutoMapper;
using Logic.Logic.PricingLogic.Interfaces;
using Models.Dtos.Pircing;
using Models.Entities.Pricing;
using Repository.Repository.PricingRepository.Interfaces;

namespace Logic.Logic.PricingLogic
{
    public class OvipPriceListLogic : IOvipPriceListLogic
    {
        private readonly IOvipPriceListRepository _repo;
        private readonly IMapper _mapper;

        public OvipPriceListLogic(
            IOvipPriceListRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public Task<List<OvipPriceList>> GetAllAsync()
            => _repo.GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        public Task<OvipPriceList?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipPriceList> CreateAsync(OvipPriceListCreateDto dto)
        {
            var entity = _mapper.Map<OvipPriceList>(dto);

            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipPriceList?> UpdateAsync(OvipPriceListUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.OvipPriceListId);

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