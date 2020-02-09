namespace Example_1.Application.Tests.Games.Commands
{
    using Example_1.Application.Games.Commands;
    using Example_1.Domain;
    using Example_1.Domain.Repositories;
    using FluentAssertions;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Shared;
    using Kernel.Library.Utils;
    using MediatR;
    using NSubstitute;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class DeleteGameTests
    {
        [Fact]
        public async Task When_deleting_a_game_unrelated_to_a_venue_then_the_game_should_be_deleted()
        {
            const int id = 10;

            var gamesRepository = Substitute.For<GamesRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            var game = Game.Create("Name").WithId(id);
            gamesRepository.Find(id, default).Returns(Task.FromResult(game));

            var deleteGameHandler = new DeleteGameHandler(gamesRepository, unitOfWork);

            await deleteGameHandler.Handle(new DeleteGame
            {
                Id = id
            }, default);

            await gamesRepository.ReceivedWithAnyArgs(1).Remove(default);
            await gamesRepository.ReceivedWithAnyArgs(1).EnsureIsNotInUse(default);
            await unitOfWork.ReceivedWithAnyArgs(1).Save();
        }

        [Fact]
        public void When_trying_delete_a_game_that_does_not_existing_then_a_domain_exception_should_be_thrown()
        {
            var gamesRepository = Substitute.For<GamesRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var deleteGameHandler = new DeleteGameHandler(gamesRepository, unitOfWork);

            Func<Task<Unit>> func = async () => await deleteGameHandler.Handle(new DeleteGame(), default);

            func.Should().Throw<DomainException>().WithMessage(Game.NOT_FOUND);
        }
    }
}
