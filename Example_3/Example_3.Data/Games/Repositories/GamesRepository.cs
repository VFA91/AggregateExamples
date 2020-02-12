namespace Example_3.Data.Games.Repositories
{
    using Example_3.Domain.Games;
    using Example_3.Domain.Repositories;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GamesRepository : IGamesRepository
    {
        private readonly Example3DbContext _context;

        public GamesRepository(Example3DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task Add(Game entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
        }

        public Task<bool> AnyAsync(Expression<Func<Game, bool>> specification, CancellationToken cancellationToken)
        {
            return _context.Set<Game>().AnyAsync(specification, cancellationToken);
        }

        public async Task<Game> Find(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<Game>().FindAsync(new object[] {id}, cancellationToken);
        }

        public async Task Remove(Game entity)
        {
            await Task.Run(() => _context.Set<Game>().Remove(entity));
        }
    }
}