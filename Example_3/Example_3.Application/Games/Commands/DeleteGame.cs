namespace Example_3.Application.Games.Commands
{
    using Example_3.Domain;
    using Example_3.Domain.Games;
    using Example_3.Domain.Repositories;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Shared;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using ApplicationException = Kernel.Library.Exceptions.ApplicationException;

    public sealed class DeleteGame : IRequest
    {
        public int Id { get; }

        public DeleteGame(int id)
        {
            Id = id;
        }
    }

    public sealed class DeleteGameHandler : IRequestHandler<DeleteGame>
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IVenuesRepository _venuesRepository;

        public DeleteGameHandler(IGamesRepository gamesRepository,
            IVenuesRepository venuesRepository,
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _gamesRepository = gamesRepository ?? throw new ArgumentNullException(nameof(gamesRepository));
            _venuesRepository = venuesRepository ?? throw new ArgumentNullException(nameof(venuesRepository));
        }

        public static string IsInUse => "It is no possible to delete Game because is in use.";

        public Task<Unit> Handle(DeleteGame request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(DeleteGame request, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Find(request.Id, cancellationToken);

            if (game is null) throw new EntityNotFoundException(typeof(Game), request.Id);

            var specification = new Venue.IsInUseSpecification(game.Id).SatisfiedBy();

            var existsVenues = await _venuesRepository.AnyAsync(specification, cancellationToken);

            if (existsVenues) throw new ApplicationException(IsInUse);

            await _gamesRepository.Remove(game);

            await _unitOfWork.Save(default);

            return Unit.Value;
        }
    }
}