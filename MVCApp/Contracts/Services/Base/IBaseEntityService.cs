using Entities.Base;
using Entities.Pagination;

namespace Contracts.Services.Base
{
    public interface IBaseEntityService<TDb>
        where TDb : class, IEntityBase
    {
        IEnumerable<TDto> GetAll<TDto>();
        PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters);
        Task<TDto> GetByIdAsync<TDto>(Guid id);
        Task<TDtoResult> CreateAsync<TDtoNewEntity, TDtoResult>(TDtoNewEntity dto);
        Task<TDtoResult> UpdateAsync<TDtoUpdateEntity, TDtoResult>(TDtoUpdateEntity dto);
        Task DeleteByIdAsync(Guid id);
    }
}
