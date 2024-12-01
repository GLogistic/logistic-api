using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Entities.Pagination;

namespace BusinessLogic
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IUserRepository repository, ISettlementRepository settlementRepository, IMapperService mapperService) : base(repository, mapperService)
        {
        }
        public async Task<string> GetUserRoleById(Guid id)
        {
            var userRoles = await (_repository as IUserRepository).GetUserRolesById(id);

            return userRoles.First();
        }
        public override PagedList<TDto> GetByPage<TDto>(PaginationQueryParameters parameters)
        {
            var entities = _repository
                .GetAll()
                .Skip((parameters.page - 1) * parameters.pageSize)
                .Take(parameters.pageSize);

            var count = _repository.Count();

            var entitiesDtos = _mapperService.Map<IEnumerable<User>, IEnumerable<TDto>>(entities);

            return new PagedList<TDto>(entitiesDtos.ToList(), count, parameters.page, parameters.pageSize);
        }
    }
}
