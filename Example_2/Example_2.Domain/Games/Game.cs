namespace Example_2.Domain.Games
{
    using Kernel.Library.Shared;
    using Kernel.Library.Validations;
    using System;
    using System.Linq.Expressions;

    public sealed partial class Game : AggregateRoot
    {
        public const int NAME_MAX_LENGTH = 50;
        public const string NAME_MUST_UNIQUE = "The Name must be unique for Game.";
        public const string IS_IN_USE = "It is no possible to delete Game because is in use.";
        public const string NOT_FOUND = "The Game requested wasn't found.";

        public string Name { get; private set; }

        private Game(string name)
        {
            SetName(name);
        }

        public static Game Create(string name) => new Game(name);

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
