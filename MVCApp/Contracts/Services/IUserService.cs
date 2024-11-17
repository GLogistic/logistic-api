using Contracts.Services.Base;
using Entities;
namespace Contracts.Services
{
    public interface IUserService : IBaseEntityService<User>
    {
        public Task<string> GetUserRoleById(Guid id);
    }
}
