namespace Example_2.Domain.Games.DomainServices
{
    using Example_2.Domain.Repositories;
    using Kernel.Library.Exceptions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class DeleteGame : IDeleteGame
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IVenuesRepository _venuesRepository;

        public DeleteGame(IGamesRepository gamesRepository,
            IVenuesRepository venuesRepository)
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _venuesRepository = venuesRepository ?? throw new ArgumentNullException(nameof(venuesRepository));
        }

        public async Task Execute(int gameId, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Find(gameId, cancellationToken);

            if (game is null) throw new DomainException(Game.NOT_FOUND);

            var specification = new Game.IsInUseSpecification(game).SatisfiedBy();

            var venues = await _venuesRepository.AnyAsync(specification, cancellationToken);

            if (venues) throw new DomainException(Game.IS_IN_USE);

            await _gamesRepository.Remove(game);
        }
    }
}
