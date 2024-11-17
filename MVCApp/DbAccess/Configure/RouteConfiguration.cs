using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbAccess.Configure
{
    public class RouteConfiguration : IEntityTypeConfiguration<Route>
    {
        public void Configure(EntityTypeBuilder<Route> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(r => r.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder
                .HasOne(r => r.StartSettlement)
                .WithMany(s => s.RouteStartSettlements)
                .HasForeignKey(r => r.StartSettlementId)
                .OnDelete(DeleteBehavior.Cascade);

            builder
                .HasOne(r => r.EndSettlement)
                .WithMany(s => s.RouteEndSettlements)
                .HasForeignKey(r => r.EndSettlementId)
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .ToTable(r => r.HasCheckConstraint("CK_Route_DifferentSettlements", "[StartSettlementId] <> [EndSettlementId]"));
        }
    }
}
