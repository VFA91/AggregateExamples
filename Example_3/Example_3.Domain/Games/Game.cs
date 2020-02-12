namespace Example_3.Domain.Games
{
    using Kernel.Library.Shared;
    using Kernel.Library.Validations;
    using System;
    using System.Linq.Expressions;

    public sealed class Game : AggregateRoot
    {
        public const int NAME_MAX_LENGTH = 50;

        private Game(string name)
        {
            SetName(name);
        }

        public string Name { get; private set; }

        public static Game Create(string name)
        {
            return new Game(name);
        }

        public void SetName(string name)
        {
            DomainPreconditions.NotNull(name, nameof(name));
            DomainPreconditions.LongerThan(name, NAME_MAX_LENGTH, nameof(name));

            Name = name;
        }

        public class IsUniqueSpecification : Specification<Game>
        {
            private readonly Game _game;
            private readonly string _newName;

            public IsUniqueSpecification(Game game, string newName)
            {
                _game = game ?? throw new ArgumentNullException(nameof(game));
                _newName = newName ?? throw new ArgumentNullException(nameof(newName));
            }

            public override Expression<Func<Game, bool>> SatisfiedBy()
            {
                return game => game.Name == _newName && game.Id != _game.Id;
            }
        }
    }
}