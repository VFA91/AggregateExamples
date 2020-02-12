namespace Example_3.Application.Tests.Games.Commands
{
    using Example_3.Application.Games.Commands;
    using Example_3.Application.Games.Validations;
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

    public sealed class UpdateGameTests
    {
        [Fact]
        public void When_editing_a_game_that_not_found_then_an_entity_not_found_exception_should_be_thrown()
        {
            var id = 3;
            var gamesRepository = Substitute.For<IGamesRepository>();
            var updateGameHandler = new UpdateGameHandler(gamesRepository, Substitute.For<IUnitOfWork>());

            Func<Task<Unit>> func = async () => await updateGameHandler.Handle(new UpdateGame(3, "Name"), default);

            func.Should().Throw<EntityNotFoundException>().WithMessage($"Entity of type {nameof(Game)} with id {id} was not found.");
        }

        [Fact]
        public void When_editing_a_game_with_existing_name_then_an_application_exception_should_be_thrown()
        {
            var id = 3;
            var name = "Name";
            var gamesRepository = Substitute.For<IGamesRepository>();
            var game = Game.Create(name).WithId(id);
            gamesRepository.Find(id, default).Returns(Task.FromResult(game));
            gamesRepository.AnyAsync(Arg.Any<Expression<Func<Game, bool>>>(), default).Returns(Task.FromResult(true));
            var updateGameHandler = new UpdateGameHandler(gamesRepository, Substitute.For<IUnitOfWork>());

            Func<Task<Unit>> func = async () => await updateGameHandler.Handle(new UpdateGame(game.Id, "Name2"), default);

            func.Should().Throw<ApplicationException>().WithMessage(Messages.NameMustUnique);
            game.Name.Should().Be(name);
        }

        [Fact]
        public async Task When_updating_an_existing_game_with_a_name_valid_then_a_new_game_should_be_created()
        {
            var id = 3;
            var newName = "Name2";
            var gamesRepository = Substitute.For<IGamesRepository>();
            var game = Game.Create("Name").WithId(id);
            gamesRepository.Find(id, default).Returns(Task.FromResult(game));
            var updateGameHandler = new UpdateGameHandler(gamesRepository, Substitute.For<IUnitOfWork>());

            var result = await updateGameHandler.Handle(new UpdateGame(game.Id, newName), default);

            game.Name.Should().Be(newName);
            await gamesRepository.ReceivedWithAnyArgs(1).AnyAsync(default, default);
            result.Should().Be(Unit.Value);
        }
    }
}