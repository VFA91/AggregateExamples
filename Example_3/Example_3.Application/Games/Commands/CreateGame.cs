namespace Example_3.Application.Games.Commands
{
    using Example_3.Application.Games.Validations;
    using Example_3.Domain.Games;
    using Example_3.Domain.Repositories;
    using Kernel.Library.Shared;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ApplicationException = Kernel.Library.Exceptions.ApplicationException;

    public sealed class CreateGame : IRequest
    {
        public string Name { get; }

        public CreateGame(string name)
        {
            Name = name;
        }
    }

    public sealed class CreateGameHandler : IRequestHandler<CreateGame>
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGameHandler(IGamesRepository gamesRepository,
            IUnitOfWork unitOfWork)
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(CreateGame request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(CreateGame request, CancellationToken cancellationToken)
        {
            var game = Game.Create(request.Name);

            var specification = new Game.IsUniqueSpecification(game, request.Name).SatisfiedBy();

            var anyGame = await _gamesRepository.AnyAsync(specification, cancellationToken);

            if (anyGame) throw new ApplicationException(Messages.NameMustUnique);

            await _unitOfWork.Save(default);

            return Unit.Value;
        }
    }
}