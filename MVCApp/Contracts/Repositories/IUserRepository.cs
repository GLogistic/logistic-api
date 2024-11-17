using Contracts.Repositories.Base;
using Entities;

namespace Contracts.Repositories
{
    public interface IUserRepository : IRepositoryBase<User>
    {
        Task<User> FindByEmailAsync(string email);
        Task<IEnumerable<string>> GetUserRolesAsync(User user);
        Task<User> CreateAsync(User user, string password, IEnumerable<string> roles);
        Task<bool> CheckPasswordAsync(User user, string password);
    }
}
