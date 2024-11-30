using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Exceptions;
using Entities.Pagination;

namespace BusinessLogic
{
    public class SettlementService : BaseService<Settlement>, ISettlementService
    {
        private readonly IRouteRepository _routeRepository;
        public SettlementService(ISettlementRepository repository, IRouteRepository routeRepository, IMapperService mapperService) : base(repository, mapperService)
        {
            _routeRepository = routeRepository;
        }
        public override async Task DeleteByIdAsync(Guid id)
        {
            var transaction = await _repository.CreateTransactionAsync();

            var settlement = await _repository.FindByIdAsync(id);

            if (settlement == null)
                throw new NotFoundException("Settlement not found.");

            var routesByStartSettlement = _routeRepository.FindByCondition(r => r.StartSettlementId == settlement.Id).ToArray();

            for (var i = 0; i < routesByStartSettlement.Length; i++)
            {
                if (routesByStartSettlement[i] == null)
                    continue;

                await _routeRepository.DeleteAsync(routesByStartSettlement[i]);
            }

            var routesByEndSettlement = _routeRepository.FindByCondition(r => r.EndSettlementId == settlement.Id && r.StartSettlementId != settlement.Id).ToArray();

            for (var i = 0; i < routesByEndSettlement.Length; i++)
            {
                if (routesByEndSettlement[i] == null)
                    continue;

                await _routeRepository.DeleteAsync(routesByEndSettlement[i]);
            }

            await _repository.SaveChangesAsync();

            await _repository.DeleteAsync(settlement);

            await transaction.CommitAsync();
        }

        public PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters, string? title)
        {
            var entities = _repository
                .GetAll()
                .Where(s => s.Title.Contains(title ?? ""))
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var entitiesDtos = _mapperService.Map<IEnumerable<Settlement>, IEnumerable<TDto>>(entities);

            return new PagedList<TDto>(entitiesDtos.ToList(), count, parameters.page, parameters.pageSize);

        }
    }
}
