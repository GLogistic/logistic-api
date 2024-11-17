using BusinessLogic.Base;
using Contracts.Mapper;
using Contracts.Repositories;
using Contracts.Services;
using Entities;

namespace BusinessLogic
{
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(IUserRepository repository, ISettlementRepository settlementRepository, IMapperService mapperService) : base(repository, mapperService)
        {
        }
    }
}
