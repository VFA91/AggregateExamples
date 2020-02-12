namespace Example_1.Domain
{
    using Kernel.Library.Shared;
    using Kernel.Library.Validations;
    using System;
    using System.Linq.Expressions;

    public sealed class Venue : AggregateRoot
    {
        public const int NAME_MAX_LENGTH = 50;

        public string Name { get; private set; }
        public int GameId { get; }

        private Venue(int gameId, string name)
        {
            GameId = gameId;

            SetName(name);
        }

        public static Venue Create(int gameId, string name) => new Venue(gameId, name);

        public void SetName(string name)
        {
            DomainPreconditions.NotNull(name, nameof(name));
            DomainPreconditions.LongerThan(name, NAME_MAX_LENGTH, nameof(name));

            Name = name;
        }
        
        public class IsInUseSpecification : Specification<Venue>
        {
            private readonly int _gameId;

            public IsInUseSpecification(int gameIdId)
            {
                _gameId = gameIdId;
            }

            public override Expression<Func<Venue, bool>> SatisfiedBy()
            {
                return venue => venue.GameId == _gameId;
            }
        }
    }
}
