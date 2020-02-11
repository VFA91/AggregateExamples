namespace Example_2.Application.Games.Commands
{
    using Example_2.Domain.Games.DomainServices;
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
        private readonly ICreateNewGame _createNewGame;
        private readonly IUnitOfWork _unitOfWork;

        public CreateGameHandler(ICreateNewGame createNewGame,
            IUnitOfWork unitOfWork)
        {
            _createNewGame = createNewGame ?? throw new ArgumentNullException(nameof(createNewGame));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(CreateGame request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(CreateGame request, CancellationToken cancellationToken)
        {
            await _createNewGame.Execute(request.Name, cancellationToken);

            await _unitOfWork.Save(default);

            return Unit.Value;
        }
    }
}
