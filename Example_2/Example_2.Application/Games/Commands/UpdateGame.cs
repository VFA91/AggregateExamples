namespace Example_2.Application.Games.Commands
{
    using Example_2.Domain.Games.DomainServices;
    using Kernel.Library.Shared;
    using MediatR;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class UpdateGame : IRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public sealed class UpdateGameHandler : IRequestHandler<UpdateGame>
    {
        private readonly IChangeName _changeName;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateGameHandler(IChangeName changeName,
            IUnitOfWork unitOfWork)
        {
            _changeName = changeName ?? throw new ArgumentNullException(nameof(changeName));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public Task<Unit> Handle(UpdateGame request, CancellationToken cancellationToken)
        {
            if (request is null) throw new ArgumentNullException(nameof(request));

            return HandleAsync(request, cancellationToken);
        }

        private async Task<Unit> HandleAsync(UpdateGame request, CancellationToken cancellationToken)
        {
            await _changeName.Execute(request.Id, request.Name, cancellationToken);

            await _unitOfWork.Save(default);

            return Unit.Value;
        }
    }
}
