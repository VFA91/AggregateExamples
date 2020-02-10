namespace Example_2.Domain.Games.DomainServices
{
    using Example_2.Domain.Repositories;
    using Kernel.Library.Exceptions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class ChangeName : IChangeName
    {
        private readonly IGamesRepository _gamesRepository;

        public ChangeName(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
        }

        public async Task Execute(int gameId, string name, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Find(gameId, cancellationToken);

            if (game is null) throw new DomainException(Game.NOT_FOUND);

            var specification = new Game.IsUniqueSpecification(game).SatisfiedBy();

            var anyGame = await _gamesRepository.AnyAsync(specification, cancellationToken);

            if (anyGame) throw new DomainException(Game.NAME_MUST_UNIQUE);

            game.SetName(name);
        }
    }
}
