using Contracts.Repositories;
using DbAccess.Context;
using DbAccess.Repositories.Base;
using Entities;
using Entities.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DbAccess.Repositories
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        private readonly UserManager<User> _userManager;
        public UserRepository(LogisticContext context, UserManager<User> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public override IQueryable<User> GetAllWithDependencies() =>
            _context.Users
                .AsNoTracking();
        public override IQueryable<User> FindByCondition(Expression<Func<User, bool>> condition) =>
            _userManager.Users.AsNoTracking().Where(condition);
        public override async Task<User> FindByIdAsync(Guid id)
        {
            var entity = await _userManager.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

            if (entity == null)
                throw new NotFoundException("Entity is not found.");

            return entity;
        }
        public async Task<User> FindByEmailAsync(string email)
        {
            var entity = await _userManager.FindByEmailAsync(email);

            if (entity == null)
                throw new NotFoundException("Entity is not found.");

            return entity;
        }
        public override IQueryable<User> GetAll() =>
            _userManager.Users.AsNoTracking();
        public async Task<IEnumerable<string>> GetUserRolesAsync(User user) => await _userManager.GetRolesAsync(user);
        public async Task<User> CreateAsync(User user, string password, IEnumerable<string> roles)
        {
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            var userByEmail = await _userManager.FindByEmailAsync(user.Email);

            result = await _userManager.AddToRolesAsync(userByEmail, roles);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);

            return user;
        }
        public override async Task<User> UpdateAsync(User entity)
        {
            var existingUser = _context.Users.FirstOrDefault(u => u.Id == entity.Id);

            if (existingUser == null)
                throw new NotFoundException("Entity is not found.");

            existingUser.FirstName = entity.FirstName;
            existingUser.LastName = entity.LastName;
            existingUser.UserName = entity.UserName;
            existingUser.Email = entity.Email;

            var result = await _userManager.UpdateAsync(existingUser);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);
            return existingUser;
        }
        public override async Task DeleteAsync(User entity)
        {
            var result = await _userManager.DeleteAsync(entity);

            if (!result.Succeeded)
                throw new Exception(result.Errors.First().Description);
        }
        public override async Task DeleteByIdAsync(Guid id)
        {
            var entity = await _userManager.FindByIdAsync(id.ToString());

            if (entity == null)
                throw new NotFoundException("Entity is not found.");

            await _userManager.DeleteAsync(entity);
        }

        public async Task<bool> CheckPasswordAsync(User user, string password) => await _userManager.CheckPasswordAsync(user, password);
    }
}
