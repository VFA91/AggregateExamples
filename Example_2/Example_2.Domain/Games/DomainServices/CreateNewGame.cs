namespace Example_2.Domain.Games.DomainServices
{
    using Example_2.Domain.Repositories;
    using Kernel.Library.Exceptions;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateNewGame : ICreateNewGame
    {
        private readonly IGamesRepository _gamesRepository;

        public CreateNewGame(IGamesRepository gamesRepository)
        {
            _gamesRepository = gamesRepository ?? throw new System.ArgumentNullException(nameof(gamesRepository));
        }

        public async Task<Game> Execute(string name, CancellationToken cancellationToken)
        {
            var game = Game.Create(name);

            var specification = new Game.IsUniqueSpecification(game).SatisfiedBy();

            bool anyGame = await _gamesRepository.AnyAsync(specification, cancellationToken);

            if (anyGame) throw new DomainException(Game.NAME_MUST_UNIQUE);

            return game;
        }
    }
}
