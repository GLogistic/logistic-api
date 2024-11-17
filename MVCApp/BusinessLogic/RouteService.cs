using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Exceptions;
using Entities.Models.DTOs;
using Entities.Pagination;

namespace BusinessLogic
{
    public class RouteService : BaseService<Route>, IRouteService
    {
        private readonly ISettlementRepository _settlementRepository;
        public RouteService(IRouteRepository repository, ISettlementRepository settlementRepository, IMapperService mapperService) : base(repository, mapperService)
        {
            _settlementRepository = settlementRepository;
        }

        public override PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters)
        {
            var routes = _repository
                .GetAllWithDependencies()
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var routeDtos = _mapperService.Map<IEnumerable<Route>, IEnumerable<TDto>>(routes);

            return new PagedList<TDto>(routeDtos.ToList(), count, parameters.page, parameters.pageSize);
        }
        public override async Task<TDtoResult> CreateAsync<TDtoNewEntity, TDtoResult>(TDtoNewEntity dto)
        {
            var typedDto = dto as RouteCreateDto;

            if (typedDto == null)
                throw new BadRequestException("Dto is wrong format.");

            var startSettlement = await _settlementRepository.FindByIdAsync(typedDto.StartSettlementId);
            var endSettlement = await _settlementRepository.FindByIdAsync(typedDto.EndSettlementId);

            var newRoute = _mapperService.Map<RouteCreateDto, Route>(typedDto);

            newRoute.StartSettlement = startSettlement;
            newRoute.EndSettlement = endSettlement;

            return await base.CreateAsync<Route, TDtoResult>(newRoute);
        }
        public override async Task<TDtoResult> UpdateAsync<TDtoUpdateEntity, TDtoResult>(TDtoUpdateEntity dto)
        {
            var typedDto = dto as RouteUpdateDto;

            if (typedDto == null)
                throw new BadRequestException("Dto is wrong format.");

            var startSettlement = await _settlementRepository.FindByIdAsync(typedDto.StartSettlementId);
            var endSettlement = await _settlementRepository.FindByIdAsync(typedDto.EndSettlementId);

            var updateRoute = _mapperService.Map<RouteUpdateDto, Route>(typedDto);

            updateRoute.StartSettlement = startSettlement;
            updateRoute.EndSettlement = endSettlement;

            updateRoute = await _repository.UpdateAsync(updateRoute);

            return _mapperService.Map<Route, TDtoResult>(updateRoute);
        }
    }
}
