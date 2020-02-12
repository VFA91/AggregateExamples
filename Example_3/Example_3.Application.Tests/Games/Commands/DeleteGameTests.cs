namespace Example_3.Application.Tests.Games.Commands
{
    using Example_3.Application.Games.Commands;
    using Example_3.Domain;
    using Example_3.Domain.Games;
    using Example_3.Domain.Repositories;
    using FluentAssertions;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Shared;
    using Kernel.Library.Utils;
    using MediatR;
    using NSubstitute;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Xunit;
    using ApplicationException = Kernel.Library.Exceptions.ApplicationException;

    public sealed class DeleteGameTests
    {
        [Fact]
        public async Task When_deleting_a_game_related_with_a_venue_then_an_application_exception_should_be_thrown()
        {
            var gameId = 5;
            var gamesRepository = Substitute.For<IGamesRepository>();
            var venuesRepository = Substitute.For<IVenuesRepository>();
            var deleteGameHandler = new DeleteGameHandler(gamesRepository, venuesRepository, Substitute.For<IUnitOfWork>());

            var game = Game.Create("Name").WithId(gameId);
            gamesRepository.Find(gameId, default).Returns(Task.FromResult(game));
            venuesRepository.AnyAsync(Arg.Any<Expression<Func<Venue, bool>>>(), default).Returns(Task.FromResult(true));

            Func<Task<Unit>> func = async () => await deleteGameHandler.Handle(new DeleteGame(game.Id), default);

            func.Should().Throw<ApplicationException>().WithMessage(DeleteGameHandler.IsInUse);
            await gamesRepository.ReceivedWithAnyArgs(0).Remove(default);
        }

        [Fact]
        public async Task When_deleting_a_game_unrelated_to_a_venue_then_the_game_should_be_deleted()
        {
            var gameId = 5;
            var gamesRepository = Substitute.For<IGamesRepository>();
            var venuesRepository = Substitute.For<IVenuesRepository>();
            var deleteGameHandler = new DeleteGameHandler(gamesRepository, venuesRepository, Substitute.For<IUnitOfWork>());

            var game = Game.Create("Name").WithId(gameId);
            gamesRepository.Find(gameId, default).Returns(Task.FromResult(game));

            var result = await deleteGameHandler.Handle(new DeleteGame(game.Id), default);

            result.Should().Be(Unit.Value);
            await venuesRepository.ReceivedWithAnyArgs(1).AnyAsync(default, default);
            await gamesRepository.ReceivedWithAnyArgs(1).Remove(default);
        }

        [Fact]
        public void When_trying_delete_a_game_that_does_not_existing_then_an_entity_not_found_exception_should_be_thrown()
        {
            var id = 3;
            var gamesRepository = Substitute.For<IGamesRepository>();
            var venuesRepository = Substitute.For<IVenuesRepository>();
            var deleteGameHandler =
                new DeleteGameHandler(gamesRepository, venuesRepository, Substitute.For<IUnitOfWork>());

            Func<Task<Unit>> func = async () => await deleteGameHandler.Handle(new DeleteGame(3), default);

            func.Should().Throw<EntityNotFoundException>().WithMessage($"Entity of type {nameof(Game)} with id {id} was not found.");
        }
    }
}