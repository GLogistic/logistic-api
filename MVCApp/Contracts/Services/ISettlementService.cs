using Contracts.Services.Base;
using Entities;
using Entities.Pagination;
namespace Contracts.Services
{
    public interface ISettlementService : IBaseEntityService<Settlement>
    {
        new PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters, string? title);
    }
}
