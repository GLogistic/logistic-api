using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbAccess.Configure
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityRole<Guid>> builder)
        {
            builder.HasData(
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "Admin",
                NormalizedName = "ADMIN"
            },
            new IdentityRole<Guid>
            {
                Id = Guid.NewGuid(),
                Name = "User",
                NormalizedName = "USER"
            });
        }
    }
}
