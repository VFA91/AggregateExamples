namespace Example_2.Data.Venues.Entities
{
    using Example_2.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EntityConfiguration : IEntityTypeConfiguration<Venue>
    {
        public void Configure(EntityTypeBuilder<Venue> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo($"{nameof(Venue)}_hilo");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(Venue.NAME_MAX_LENGTH);
            builder.Property(e => e.GameId).IsRequired();
        }
    }
}
