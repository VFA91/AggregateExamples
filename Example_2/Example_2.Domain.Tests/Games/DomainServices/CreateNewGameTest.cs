namespace Example_2.Domain.Tests.Games.DomainServices
{
    using Example_2.Domain.Games;
    using Example_2.Domain.Games.DomainServices;
    using Example_2.Domain.Repositories;
    using FluentAssertions;
    using Kernel.Library.Exceptions;
    using NSubstitute;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class CreateNewGameTest
    {
        private const string NAME = "Name";

        [Fact]
        public void When_creating_a_game_with_existing_name_then_a_domain_exception_should_be_thrown()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();

            gamesRepository.AnyAsync(Arg.Any<Expression<Func<Game, bool>>>(), default).Returns(Task.FromResult(true));

            var createNewGame = new CreateNewGame(gamesRepository);

            Func<Task<Game>> action = async () => await createNewGame.Execute(NAME, default);

            action.Should().Throw<DomainException>().WithMessage(Game.NAME_MUST_UNIQUE);
        }

        [Fact]
        public async Task When_creating_a_game_with_a_name_valid_then_a_new_game_should_be_created()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();

            gamesRepository.AnyAsync(Arg.Any<Expression<Func<Game, bool>>>(), default).Returns(Task.FromResult(false));

            var createNewGame = new CreateNewGame(gamesRepository);

            var result = await createNewGame.Execute(NAME, default);

            result.Should().NotBeNull();
        }
    }
}
