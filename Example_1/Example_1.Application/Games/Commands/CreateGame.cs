namespace Example_1.Application.Games.Commands
{
    using Example_1.Domain;
    using Example_1.Domain.Repositories;
    using Kernel.Library.Shared;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class CreateGame : IRequest
    {
        public string Name { get; set; }
    }

    public sealed class CreateGameHandler : IRequestHandler<CreateGame>
    {
        private readonly GamesRepository _gamesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGameHandler(GamesRepository gamesRepository,
            IUnitOfWork unitOfWork)
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(CreateGame request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(CreateGame request, CancellationToken cancellationToken)
        {
            var game = Game.Create(request.Name);

            await _gamesRepository.Add(game, cancellationToken);

            await _unitOfWork.Save();

            return Unit.Value;
        }
    }
}
