namespace Example_2.Domain.Tests.Games.DomainServices
{
    using Example_2.Domain.Games;
    using Example_2.Domain.Games.DomainServices;
    using Example_2.Domain.Repositories;
    using FluentAssertions;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Utils;
    using NSubstitute;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class ChangeNameTest
    {
        private const string NAME = "Name";
        private const int ID = 1;

        [Fact]
        public void When_editing_a_game_with_existing_name_then_a_domain_exception_should_be_thrown()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();
            var changeName = new ChangeName(gamesRepository);

            var game = Game.Create(NAME).WithId(ID);
            gamesRepository.Find(ID, default).Returns(Task.FromResult(game));
            gamesRepository.AnyAsync(Arg.Any<Expression<Func<Game, bool>>>(), default).Returns(Task.FromResult(true));

            Func<Task> action = async () => await changeName.Execute(ID, "Name2", default);

            action.Should().Throw<DomainException>().WithMessage(Game.NAME_MUST_UNIQUE);
        }

        [Fact]
        public void When_editing_a_game_that_not_found_then_a_domain_exception_should_be_thrown()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();

            var changeName = new ChangeName(gamesRepository);

            Func<Task> action = async () => await changeName.Execute(0, "Test", default);

            action.Should().Throw<DomainException>().WithMessage(Game.NOT_FOUND);
        }

        [Fact]
        public async Task When_editing_a_game_with_a_name_valid_then_the_game_should_be_edited()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();
            var changeName = new ChangeName(gamesRepository);

            var game = Game.Create(NAME).WithId(ID);
            gamesRepository.Find(ID, default).Returns(Task.FromResult(game));
            gamesRepository.AnyAsync(Arg.Any<Expression<Func<Game, bool>>>(), default).Returns(Task.FromResult(false));

            await changeName.Execute(game.Id, "Name2", default);

            game.Name.Should().Be("Name2");
        }
    }
}
