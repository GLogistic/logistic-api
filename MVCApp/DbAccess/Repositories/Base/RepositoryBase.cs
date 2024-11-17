using Contracts.Repositories.Base;
using DbAccess.Context;
using Entities.Base;
using Entities.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace DbAccess.Repositories.Base
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class, IEntityBase
    {
        protected readonly LogisticContext _context;

        public RepositoryBase(LogisticContext context)
        {
            _context = context;
        }

        public int Count() => _context.Set<T>().Count();

        public abstract IQueryable<T> GetAllWithDependencies();

        public virtual IQueryable<T> FindByCondition(Expression<Func<T, bool>> condition) =>
            _context.Set<T>().AsNoTracking().Where(condition);
        public virtual async Task<T> FindByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().FirstOrDefaultAsync((entity) => entity.Id == id);

            if (entity == null)
                throw new NotFoundException("Entity is not found.");

            return entity;
        }
        public virtual IQueryable<T> GetAll() =>
            _context.Set<T>().AsNoTracking();

        public virtual async Task<T> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await SaveChangesAsync();
            return entity;
        }

        public virtual async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }
        public virtual async Task DeleteByIdAsync(Guid id)
        {
            var entity = await _context.Set<T>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (entity == null)
                throw new NotFoundException("Entity is not found.");

            _context.Set<T>().Remove(entity);
            await SaveChangesAsync();
        }

        public virtual void SaveChanges() => _context.SaveChanges();
        public virtual Task SaveChangesAsync() => _context.SaveChangesAsync();

        public async Task<IDbContextTransaction> CreateTransactionAsync() => await _context.Database.BeginTransactionAsync();

        public IDbContextTransaction CreateTransaction() => _context.Database.BeginTransaction();
    }
}
