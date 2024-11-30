using Contracts.Services.Base;
using Entities;
using Entities.Pagination;
namespace Contracts.Services
{
    public interface ISettlementService : IBaseEntityService<Settlement>
    {
        PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters, string? startSettlementTitle);
    }
}
