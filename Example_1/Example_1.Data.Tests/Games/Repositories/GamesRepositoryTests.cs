namespace Example_1.Data.Tests.Games.Repositories
{
    using Example_1.Data.Games.Repositories;
    using Example_1.Domain;
    using FluentAssertions;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Utils;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class GamesRepositoryTests
    {
        private const string NAME = "Name";

        [Fact]
        public async Task When_creating_a_new_game_with_existing_name_then_a_domain_exception_should_be_thrown()
        {
            Example1DbContext context = GetContext();

            var game = Game.Create(NAME).WithId(5);
            context.Games.Add(game);
            await context.Save();

            var gamesRepository = new GamesRepository(context);

            Func<Task> action = async () => await gamesRepository.Add(Game.Create(NAME), default);

            action.Should().Throw<DomainException>().WithMessage(Game.NAME_MUST_UNIQUE);
        }

        [Fact]
        public void When_creating_a_new_game_with_a_name_valid_then_a_new_game_should_be_created()
        {
            Example1DbContext context = GetContext();

            var game = Game.Create(NAME).WithId(5);
            context.Games.Add(game);

            var gamesRepository = new GamesRepository(context);

            Func<Task> action = async () => await gamesRepository.Add(Game.Create("Name 2"), default);

            action.Should().NotThrow();
        }

        [Fact]
        public void When_updating_an_existing_game_with_the_same_values_then_the_uniqueness_validation_should_be_ok()
        {
            Example1DbContext context = GetContext();

            var game = Game.Create(NAME).WithId(5);
            context.Games.Add(game);

            var gamesRepository = new GamesRepository(context);

            Func<Task> func = async () => await gamesRepository.EnsureUniqueness(game);

            func.Should().NotThrow();
        }

        [Fact]
        public async Task When_deleting_a_game_related_to_a_venue_then_a_domain_exception_should_be_thrown()
        {
            Example1DbContext context = GetContext();

            var game = Game.Create(NAME).WithId(5);
            context.Games.Add(game);
            var venue = Venue.Create(5, "Venue");
            context.Venues.Add(venue);
            await context.Save();

            var gamesRepository = new GamesRepository(context);

            Func<Task> func = async () => await gamesRepository.Remove(game);

            func.Should().Throw<DomainException>().WithMessage(Game.IS_IN_USE);
        }

        [Fact]
        public void When_deleting_a_game_unrelated_to_a_venue_then_the_game_should_be_created()
        {
            Example1DbContext context = GetContext();

            var game = Game.Create(NAME).WithId(5);
            context.Games.Add(game);

            var gamesRepository = new GamesRepository(context);

            Func<Task> func = async () => await gamesRepository.Remove(game);

            func.Should().NotThrow();
        }

        private static Example1DbContext GetContext()
        {
            var options = new DbContextOptionsBuilder<Example1DbContext>()
                           .UseInMemoryDatabase($"Example1DatabaseForTesting{Guid.NewGuid()}")
                           .Options;

            return new Example1DbContext(options);
        }
    }
}
