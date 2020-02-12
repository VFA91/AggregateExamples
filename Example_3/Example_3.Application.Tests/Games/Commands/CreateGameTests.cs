namespace Example_3.Application.Tests.Games.Commands
{
    using Example_3.Application.Games.Commands;
    using Example_3.Application.Games.Validations;
    using Example_3.Domain.Games;
    using Example_3.Domain.Repositories;
    using FluentAssertions;
    using Kernel.Library.Shared;
    using MediatR;
    using NSubstitute;
    using System;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Xunit;
    using ApplicationException = Kernel.Library.Exceptions.ApplicationException;

    public sealed class CreateGameTests
    {
        [Fact]
        public void When_creating_a_game_with_existing_name_then_an_application_exception_should_be_thrown()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();
            gamesRepository.AnyAsync(Arg.Any<Expression<Func<Game, bool>>>(), default).Returns(Task.FromResult(true));

            var createGameHandler = new CreateGameHandler(gamesRepository, Substitute.For<IUnitOfWork>());

            Func<Task<Unit>> action = async () => await createGameHandler.Handle(new CreateGame("Name"), default);

            action.Should().Throw<ApplicationException>().WithMessage(Messages.NameMustUnique);
        }

        [Fact]
        public async Task When_creating_a_new_game_with_a_name_valid_then_a_new_game_should_be_created()
        {
            var gamesRepository = Substitute.For<IGamesRepository>();
            var createGameHandler = new CreateGameHandler(gamesRepository, Substitute.For<IUnitOfWork>());

            var result = await createGameHandler.Handle(new CreateGame("Name"), default);

            await gamesRepository.ReceivedWithAnyArgs(1).AnyAsync(default, default);
            result.Should().Be(Unit.Value);
        }
    }
}