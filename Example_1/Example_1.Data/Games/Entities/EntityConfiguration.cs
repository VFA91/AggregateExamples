namespace Example_1.Data.Games.Entities
{
    using Example_1.Domain;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class EntityConfiguration : IEntityTypeConfiguration<Game>
    {
        public void Configure(EntityTypeBuilder<Game> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).UseHiLo($"{nameof(Game)}_hilo");
            builder.Property(e => e.Name).IsRequired().HasMaxLength(Game.NAME_MAX_LENGTH);
        }
    }
}
