using Contracts.Repositories;
using DbAccess.Context;
using DbAccess.Repositories.Base;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class RouteRepository : RepositoryBase<Route>, IRouteRepository
    {
        public RouteRepository(LogisticContext context) : base(context) { }

        public override IQueryable<Route> GetAllWithDependencies() =>
            _context.Routes
                .AsNoTracking()
                .Include(r => r.StartSettlement)
                .Include(r => r.EndSettlement);
    }
}
