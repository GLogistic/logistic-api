using Contracts.Mapper;
using Contracts.Repositories.Base;
using Contracts.Services.Base;
using Entities.Base;
using Entities.Pagination;

namespace BusinessLogic.Base
{
    public class BaseService<TDb> : IBaseEntityService<TDb>
        where TDb : class, IEntityBase
    {
        protected readonly IRepositoryBase<TDb> _repository;
        protected readonly IMapperService _mapperService;
        public BaseService(IRepositoryBase<TDb> repository, IMapperService mapperService)
        {
            _repository = repository;
            _mapperService = mapperService;
        }
        public virtual IEnumerable<TDto> GetAll<TDto>()
        {
            var allDbEntity = _repository.GetAll();

            var dtos = _mapperService.Map<IQueryable<TDb>, IEnumerable<TDto>>(allDbEntity);
            return dtos;
        }
        public virtual PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters)
        {
            var entities = _repository
                .GetAll()
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var entitiesDtos = _mapperService.Map<IEnumerable<TDb>, IEnumerable<TDto>>(entities);

            return new PagedList<TDto>(entitiesDtos.ToList(), count, parameters.page, parameters.pageSize);
        }
        public virtual async Task<TDto> GetByIdAsync<TDto>(Guid id)
        {
            var dbEntity = await _repository.FindByIdAsync(id);

            return _mapperService.Map<TDb, TDto>(dbEntity);
        }

        public virtual async Task<TDtoResult> CreateAsync<TDtoNewEntity, TDtoResult>(TDtoNewEntity dto)
        {
            var mapEntity = _mapperService.Map<TDtoNewEntity, TDb>(dto);

            var dbEntity = await _repository.CreateAsync(mapEntity);

            return _mapperService.Map<TDb, TDtoResult>(dbEntity);
        }
        public virtual async Task DeleteByIdAsync(Guid id) => await _repository.DeleteByIdAsync(id);
        public virtual async Task<TDtoResult> UpdateAsync<TDtoUpdateEntity, TDtoResult>(TDtoUpdateEntity dto)
        {
            var mapEntity = _mapperService.Map<TDtoUpdateEntity, TDb>(dto);

            var dbEntity = await _repository.UpdateAsync(mapEntity);

            return _mapperService.Map<TDb, TDtoResult>(dbEntity);
        }
    }
}
