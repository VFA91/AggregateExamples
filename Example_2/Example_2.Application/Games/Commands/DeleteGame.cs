namespace Example_2.Application.Games.Commands
{
    using Example_2.Domain.Games.DomainServices;
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
        private readonly IDeleteGame _deleteGame;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteGameHandler(IDeleteGame deleteGame,
            IUnitOfWork unitOfWork)
        {
            _deleteGame = deleteGame ?? throw new ArgumentNullException(nameof(deleteGame));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(DeleteGame request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(DeleteGame request, CancellationToken cancellationToken)
        {
            await _deleteGame.Execute(request.Id, cancellationToken);

            await _unitOfWork.Save(default);

            return Unit.Value;
        }
    }
}
