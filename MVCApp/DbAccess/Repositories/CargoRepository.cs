using Contracts.Repositories;
using DbAccess.Context;
using DbAccess.Repositories.Base;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class CargoRepository : RepositoryBase<Cargo>, ICargoRepository
    {
        public CargoRepository(LogisticContext context) : base(context) { }

        public override IQueryable<Cargo> GetAllWithDependencies() =>
            _context.Cargos
                .AsNoTracking();
    }
}
