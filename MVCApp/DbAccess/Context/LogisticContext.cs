using DbAccess.Configure;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DbAccess.Context
{
    public class LogisticContext : DbContext
    {
        public DbSet<Cargo> Cargos { get; set; }
        public DbSet<Route> Routes { get; set; }
        public DbSet<Settlement> Settlements { get; set; }

        public DbSet<User> Users { get; set; }

        public LogisticContext(DbContextOptions<LogisticContext> builder) : base(builder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new CargoConfiguration());
            modelBuilder.ApplyConfiguration(new RouteConfiguration());
            modelBuilder.ApplyConfiguration(new SettlementConfiguration());

            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.Entity<IdentityUserRole<Guid>>().HasKey(x => new { x.UserId, x.RoleId });
        }
    }
}
