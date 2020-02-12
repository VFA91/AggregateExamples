namespace Example_1.Application.Tests.Games.Commands
{
    using Example_1.Application.Games.Commands;
    using Example_1.Domain;
    using Example_1.Domain.Repositories;
    using FluentAssertions;
    using Kernel.Library.Shared;
    using Kernel.Library.Utils;
    using NSubstitute;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class UpdateGameTests
    {
        [Fact]
        public async Task When_updating_an_existing_game_with_a_name_valid_then_a_new_game_should_be_created()
        {
            const int id = 3;

            var gamesRepository = Substitute.For<GamesRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            var game = Game.Create("Name").WithId(id);
            gamesRepository.Find(id, default).Returns(Task.FromResult(game));

            var updateGameHandler = new UpdateGameHandler(gamesRepository, unitOfWork);

            await updateGameHandler.Handle(new UpdateGame
            {
                Id = id,
                Name = "Name2"
            }, default);

            game.Name.Should().Be("Name2");
            await gamesRepository.ReceivedWithAnyArgs(1).EnsureUniqueness(default);
            await unitOfWork.ReceivedWithAnyArgs(1).Save(default);
        }
    }
}
