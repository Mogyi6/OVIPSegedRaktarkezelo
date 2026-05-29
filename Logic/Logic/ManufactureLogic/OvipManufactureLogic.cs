using AutoMapper;
using Logic.Logic.ManufactureLogic.Interfaces;
using Models.Dtos.Manufacture;
using Models.Entities.Manufacture;
using Repository.Repository.ManufactureRepositroy.Interfaces;

namespace Logic.Logic.ManufactureLogic
{
    public class OvipManufactureLogic : IOvipManufactureLogic
    {
        private readonly IOvipManufactureRepository _repo;
        private readonly IMapper _mapper;

        public OvipManufactureLogic(
            IOvipManufactureRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public Task<List<OvipManufacture>> GetAllAsync()
            => _repo.GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        public Task<OvipManufacture?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // GET BY PRODUCT
        // =========================
        public Task<OvipManufacture?> GetByProductIdAsync(int productId)
            => _repo.GetByProductIdAsync(productId);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipManufacture> CreateAsync(OvipManufactureCreateDto dto)
        {
            var entity = _mapper.Map<OvipManufacture>(dto);
            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipManufacture?> UpdateAsync(OvipManufactureUpdateDto dto)
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