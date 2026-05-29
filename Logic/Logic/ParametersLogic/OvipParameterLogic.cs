using AutoMapper;
using Logic.Logic.ParametersLogic.Interfaces;
using Models.Dtos.Parameters;
using Models.Entities.Parameters;
using Repository.Repository.ParametersRepositroy.Interfaces;

namespace Logic.Logic.ParametersLogic
{
    public class OvipParameterLogic : IOvipParameterLogic
    {
        private readonly IOvipParameterRepository _repo;
        private readonly IMapper _mapper;

        public OvipParameterLogic(
            IOvipParameterRepository repo,
            IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        // =========================
        // GET ALL
        // =========================
        public Task<List<OvipParameter>> GetAllAsync()
            => _repo.GetAllAsync();

        // =========================
        // GET BY ID
        // =========================
        public Task<OvipParameter?> GetByIdAsync(int id)
            => _repo.GetByIdAsync(id);

        // =========================
        // CREATE
        // =========================
        public async Task<OvipParameter> CreateAsync(OvipParameterCreateDto dto)
        {
            var entity = _mapper.Map<OvipParameter>(dto);
            return await _repo.CreateAsync(entity);
        }

        // =========================
        // UPDATE
        // =========================
        public async Task<OvipParameter?> UpdateAsync(OvipParameterUpdateDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.OvipParameterId);

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