namespace Example_2.Domain
{
    using Kernel.Library.Shared;
    using Kernel.Library.Validations;

    public sealed class Venue : AggregateRoot
    {
        public const int NAME_MAX_LENGTH = 50;

        public string Name { get; private set; }
        public int GameId { get; set; }

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
    }
}
