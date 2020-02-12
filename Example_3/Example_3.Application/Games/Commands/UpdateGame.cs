namespace Example_3.Application.Games.Commands
{
    using Example_3.Application.Games.Validations;
    using Example_3.Domain.Games;
    using Example_3.Domain.Repositories;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Shared;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ApplicationException = Kernel.Library.Exceptions.ApplicationException;

    public sealed class UpdateGame : IRequest
    {
        public int Id { get; }
        public string Name { get; }

        public UpdateGame(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public sealed class UpdateGameHandler : IRequestHandler<UpdateGame>
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGameHandler(IGamesRepository gamesRepository,
            IUnitOfWork unitOfWork)
        {
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(UpdateGame request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(UpdateGame request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Find(request.Id, cancellationToken);

            if (game is null) throw new EntityNotFoundException(typeof(Game), request.Id);

            var specification = new Game.IsUniqueSpecification(game, request.Name).SatisfiedBy();

            var anyGame = await _gamesRepository.AnyAsync(specification, cancellationToken);

            if (anyGame) throw new ApplicationException(Messages.NameMustUnique);

            game.SetName(request.Name);

            await _unitOfWork.Save(default);

            return Unit.Value;
        }
    }
}