using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DbAccess.Configure
{
    public class CargoConfiguration : IEntityTypeConfiguration<Cargo>
    {
        public void Configure(EntityTypeBuilder<Cargo> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("NEWSEQUENTIALID()");

            builder
                .Property(c => c.Title)
                .IsRequired();

            builder
                .Property(c => c.Weight)
                .IsRequired();

            builder
                .Property(c => c.RegistrationNumber)
                .IsRequired();
        }
    }
}
