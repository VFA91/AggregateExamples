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

    public sealed class DeleteGameTest
    {
        private const string NAME = "Name";
        private const int ID = 1;

        [Fact]
        public void When_deleting_a_game_related_with_a_venue_then_a_domain_exception_should_be_thrown()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();
            var venuesRepository = Substitute.For<IVenuesRepository>();
            var deleteGame = new DeleteGame(gamesRepository, venuesRepository);

            var game = Game.Create(NAME).WithId(ID);
            gamesRepository.Find(ID, default).Returns(Task.FromResult(game));
            venuesRepository.AnyAsync(Arg.Any<Expression<Func<Venue, bool>>>(), default).Returns(Task.FromResult(true));

            Func<Task> action = async () => await deleteGame.Execute(game.Id, default);

            action.Should().Throw<DomainException>().WithMessage(Game.IS_IN_USE);
        }

        [Fact]
        public void When_deleting_a_game_that_not_exist_then_domain_exception_should_be_thrown()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();
            var venuesRepository = Substitute.For<IVenuesRepository>();
            var deleteGame = new DeleteGame(gamesRepository, venuesRepository);

            Func<Task> action = async () => await deleteGame.Execute(0, default);

            action.Should().Throw<DomainException>().WithMessage(Game.NOT_FOUND);
        }

        [Fact]
        public void When_deleting_a_game_correctly_then_game_should_be_removed()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();
            var venuesRepository = Substitute.For<IVenuesRepository>();
            var deleteGame = new DeleteGame(gamesRepository, venuesRepository);

            var game = Game.Create(NAME).WithId(ID);
            gamesRepository.Find(ID, default).Returns(Task.FromResult(game));
            venuesRepository.AnyAsync(Arg.Any<Expression<Func<Venue, bool>>>(), default).Returns(Task.FromResult(false));

            Func<Task> action = async () => await deleteGame.Execute(game.Id, default);

            action.Should().NotThrow();
        }
    }
}
