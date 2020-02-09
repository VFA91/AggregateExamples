namespace Example_1.Application.Games.Commands
{
    using Example_1.Domain;
    using Example_1.Domain.Repositories;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Shared;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class DeleteGame : IRequest
    {
        public int Id { get; set; }
    }

    public sealed class DeleteGameHandler : IRequestHandler<DeleteGame>
    {
        private readonly GamesRepository _gamesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGameHandler(GamesRepository gamesRepository,
            IUnitOfWork unitOfWork)
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(DeleteGame request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(DeleteGame request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Find(request.Id, cancellationToken);

            if (game is null) throw new DomainException(Game.NOT_FOUND);

            await _gamesRepository.Remove(game);

            await _unitOfWork.Save();

            return Unit.Value;
        }
    }
}
