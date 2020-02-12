namespace Example_1.Data.Games.Repositories
{
    using Example_1.Domain;
    using Kernel.Library.Exceptions;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class GamesRepository : Domain.Repositories.GamesRepository
    {
        private readonly Example1DbContext _context;

        public GamesRepository(Example1DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override async Task EnsureIsNotInUse(Game entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var specification = new Venue.IsInUseSpecification(entity.Id);

            var anyVenue = await _context.Set<Venue>().AnyAsync(specification.SatisfiedBy());

            if (anyVenue) throw new DomainException(Game.IS_IN_USE);
        }

        public override async Task EnsureUniqueness(Game entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var specification = new Game.IsUniqueSpecification(entity);

            var existingGames = await _context.Set<Game>().AnyAsync(specification.SatisfiedBy());

            if (existingGames) throw new DomainException(Game.NAME_MUST_UNIQUE);
        }

        public async override Task<Game> Find(int id, CancellationToken cancellationToken)
        {
            return await _context.Set<Game>().FindAsync(new object[] { id }, cancellationToken);
        }

        protected async override Task InternalAdd(Game entity, CancellationToken cancellationToken)
        {
            await _context.AddAsync(entity, cancellationToken);
        }

        protected override void InternalRemove(Game entity)
        {
            _context.Set<Game>().Remove(entity);
        }
    }
}
