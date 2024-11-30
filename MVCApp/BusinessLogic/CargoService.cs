using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Pagination;

namespace BusinessLogic
{
    public class CargoService : BaseService<Cargo>, ICargoService
    {
        public CargoService(ICargoRepository repository, IMapperService mapperService) : base(repository, mapperService)
        {
        }
        public PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters, string? titleFilter)
        {
            var entities = _repository
                .GetAll()
                .Where(s => s.Title.Contains(titleFilter ?? ""))
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var entitiesDtos = _mapperService.Map<IEnumerable<Cargo>, IEnumerable<TDto>>(entities);

            return new PagedList<TDto>(entitiesDtos.ToList(), count, parameters.page, parameters.pageSize);

        }
    }
}
