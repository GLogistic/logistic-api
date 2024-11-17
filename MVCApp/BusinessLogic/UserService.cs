using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;
using Microsoft.EntityFrameworkCore;

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
    }
}
