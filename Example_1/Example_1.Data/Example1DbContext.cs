namespace Example_1.Data
{
    using Example_1.Domain;
    using Kernel.Library.Shared;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class Example1DbContext : DbContext, IUnitOfWork
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<Venue> Venues { get; set; }

        public Example1DbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new Games.Entities.EntityConfiguration());
            modelBuilder.ApplyConfiguration(new Venues.Entities.EntityConfiguration());
        }

        public async Task Save()
        {
            try
            {
                await SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
