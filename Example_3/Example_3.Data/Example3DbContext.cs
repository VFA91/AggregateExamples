namespace Example_3.Data
{
    using Example_3.Data.Games.Entities;
    using Example_3.Domain;
    using Example_3.Domain.Games;
    using Kernel.Library.Shared;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Example3DbContext : DbContext, IUnitOfWork
    {
        public Example3DbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Game> Games { get; set; }
        public DbSet<Venue> Venues { get; set; }

        public async Task Save(CancellationToken cancellationToken)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new EntityConfiguration());
            modelBuilder.ApplyConfiguration(new Venues.Entities.EntityConfiguration());
        }
    }
}