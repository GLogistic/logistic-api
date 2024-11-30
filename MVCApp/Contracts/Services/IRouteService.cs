using Contracts.Services.Base;
using Entities;
using Entities.Pagination;

namespace Contracts.Services
{
    public interface IRouteService : IBaseEntityService<Route>
    {
        PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters, string? startSettlementTitle);
    }
}
