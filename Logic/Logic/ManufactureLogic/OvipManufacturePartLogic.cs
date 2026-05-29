using AutoMapper;
using Logic.Logic.ManufactureLogic.Interfaces;
using Models.Dtos.Manufacture;
using Models.Entities.Manufacture;
using Repository.Repository.ManufactureRepositroy.Interfaces;

namespace Logic.Logic.ManufactureLogic
{
    public class OvipManufacturePartLogic : IOvipManufacturePartLogic
    {
        private readonly IOvipManufacturePartRepository _repo;
        private readonly IMapper _mapper;

        public OvipManufacturePartLogic(IOvipManufacturePartRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public Task<List<OvipManufacturePart>> GetAllAsync()
            => _repo.GetAllAsync();

        public Task<OvipManufacturePart?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        public Task<List<OvipManufacturePart>> GetByManufactureIdAsync(int manufactureId)
            => _repo.GetByManufactureIdAsync(manufactureId);

        public Task<List<OvipManufacturePart>> GetByPartProductIdAsync(int productId)
            => _repo.GetByPartProductIdAsync(productId);

        public async Task<OvipManufacturePart> CreateAsync(OvipManufacturePartCreateDto dto)
        {
            var entity = _mapper.Map<OvipManufacturePart>(dto);
            return await _repo.CreateAsync(entity);
        }

        public async Task<OvipManufacturePart?> UpdateAsync(OvipManufacturePartUpdateDto dto)
        {
            var entity = _mapper.Map<OvipManufacturePart>(dto);
            return await _repo.UpdateAsync(entity);
        }

        public Task<bool> DeleteAsync(int id)
            => _repo.DeleteAsync(id);
    }
}