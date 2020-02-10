namespace Example_2.Data
{
    using Example_2.Domain;
    using Example_2.Domain.Games;
    using Kernel.Library.Shared;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class Example2DbContext : DbContext, IUnitOfWork
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Venue> Venues { get; set; }

        public Example2DbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Games.Entities.EntityConfiguration());
            modelBuilder.ApplyConfiguration(new Venues.Entities.EntityConfiguration());
        }

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
    }
}
