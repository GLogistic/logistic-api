using Contracts.Repositories;
using DbAccess.Context;
using DbAccess.Repositories.Base;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Repositories
{
    public class SettlementRepository : RepositoryBase<Settlement>, ISettlementRepository
    {
        public SettlementRepository(LogisticContext context) : base(context) { }

        public override IQueryable<Settlement> GetAllWithDependencies() =>
            _context.Settlements
                .AsNoTracking()
                .Include(s => s.RouteStartSettlements)
                .Include(s => s.RouteEndSettlements);
    }
}
