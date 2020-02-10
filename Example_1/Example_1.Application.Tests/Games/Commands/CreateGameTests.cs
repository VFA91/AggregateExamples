namespace Example_1.Application.Tests.Games.Commands
{
    using Example_1.Application.Games.Commands;
    using Example_1.Domain.Repositories;
    using Kernel.Library.Shared;
    using NSubstitute;
    using System.Threading.Tasks;
    using Xunit;

    public sealed class CreateGameTests
    {
        [Fact]
        public async Task When_creating_a_new_game_with_a_name_valid_then_a_new_game_should_be_created()
        {
            var gamesRepository = Substitute.For<GamesRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();

            var createGameHandler = new CreateGameHandler(gamesRepository, unitOfWork);

            await createGameHandler.Handle(new CreateGame
            {
                Name = "Name"
            }, default);

            await gamesRepository.ReceivedWithAnyArgs(1).Add(default, default);
            await gamesRepository.ReceivedWithAnyArgs(1).EnsureUniqueness(default);
        }
    }
}
