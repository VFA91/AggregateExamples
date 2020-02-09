namespace Example_2.Data.Venues.Repositories
{
    using Example_2.Domain;
    using Example_2.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class VenuesRepository : IVenuesRepository
    {
        private readonly Example2DbContext _context;

        public VenuesRepository(Example2DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Venue entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<Venue, bool>> specification, CancellationToken cancellationToken)
        {
            return _context.Set<Venue>().AnyAsync(specification, cancellationToken);
        }

        public async Task<Venue> Find(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<Venue>().FindAsync(new object[] { id }, cancellationToken);
        }

        public async Task Remove(Venue entity)
        {
            await Task.Run(() => _context.Set<Venue>().Remove(entity));
        }
    }
}
