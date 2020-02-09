namespace Example_2.Application.Games.Commands
{
    using Example_2.Domain.Games.DomainServices;
    using Example_2.Domain.Repositories;
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
        private readonly IGamesRepository _gamesRepository;
        private readonly IDeleteGame _deleteGame;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGameHandler(IGamesRepository gamesRepository,
            IDeleteGame deleteGame,
            IUnitOfWork unitOfWork)
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _deleteGame = deleteGame ?? throw new ArgumentNullException(nameof(deleteGame));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(DeleteGame request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(DeleteGame request, CancellationToken cancellationToken)
        {
            await _deleteGame.Execute(request.Id, cancellationToken);

            await _unitOfWork.Save();

            return Unit.Value;
        }
    }
}
