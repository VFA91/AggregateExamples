# AggregateExamples

<img
  src="https://i.ibb.co/X8JNLcd/uml.png"
  alt="UML"
  width="350"
  height="144"
/>

## Business Rules

* When you want to add or modify a game, there must exist only one game in the database with the same name (name not duplicated).
* When a Game is deleted, it cannot be deleted if any Venue has a relationship with the game to be deleted.

### Example 1
This example, we give the responsibility of adding, editing or deleting to the Data layer, since the repository knows the rest of the aggregates of the database.

```csharp
namespace Example_1.Domain.Repositories
{
    public abstract class GamesRepository : IRepository<Game>
    {
        public async Task Add(Game entity, CancellationToken cancellationToken)
        {
            await EnsureUniqueness(entity);
            await InternalAdd(entity, cancellationToken);
        }

        public async Task Remove(Game entity)
        {
            await EnsureIsNotInUse(entity);
            InternalRemove(entity);
        }
        public abstract Task EnsureUniqueness(Game entity);
        public abstract Task EnsureIsNotInUse(Game entity);
        protected abstract Task InternalAdd(Game entity, CancellationToken cancellationToken);
        protected abstract void InternalRemove(Game entity);
    }
}
```

I use the domain rules in the repository implementation.

```csharp
namespace Example_1.Data.Games.Repositories
{
    public sealed class GamesRepository : Domain.Repositories.GamesRepository
    {
        private readonly Example1DbContext _context;

        public GamesRepository(Example1DbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public override async Task EnsureIsNotInUse(Game entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var specification = new Game.IsInUseSpecification(entity);

            var anyVenue = await _context.Set<Venue>().AnyAsync(specification.SatisfiedBy());

            if (anyVenue) throw new DomainException(Game.IS_IN_USE);
        }

        public override async Task EnsureUniqueness(Game entity)
        {
            _ = entity ?? throw new ArgumentNullException(nameof(entity));

            var specification = new Game.IsUniqueSpecification(entity);

            var existingGames = await _context.Set<Game>().AnyAsync(specification.SatisfiedBy());

            if (existingGames) throw new DomainException(Game.NAME_MUST_UNIQUE);
        }
    }
}
```

### Example 2

I define business rules as domain services such as DeleteGame

```csharp
namespace Example_2.Domain.Games.DomainServices
{
    public sealed class DeleteGame : IDeleteGame
    {
        private readonly IGamesRepository _gamesRepository;
        private readonly IVenuesRepository _venuesRepository;

        public DeleteGame(IGamesRepository gamesRepository,
            IVenuesRepository venuesRepository)
        {
            _gamesRepository = gamesRepository;
            _venuesRepository = venuesRepository;
        }

        public async Task Execute(int gameId, CancellationToken cancellationToken)
        {
            var game = await _gamesRepository.Find(gameId, cancellationToken);

            if (game is null) throw new DomainException(Game.NOT_FOUND);

            var specification = new Game.IsInUseSpecification(game).SatisfiedBy();

            var venues = await _venuesRepository.AnyAsync(specification, cancellationToken);

            if (venues) throw new DomainException(Game.IS_IN_USE);

            await _gamesRepository.Remove(game);
        }
    }
}
```

### Example 3

I define business rules as application services such as DeleteGame

```csharp
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
```
