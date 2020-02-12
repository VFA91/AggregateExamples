namespace Example_3.Domain.Tests
{
    using Example_3.Domain.Games;
    using FluentAssertions;
    using Kernel.Library.Exceptions;
    using Kernel.Library.Validations;
    using System;
    using Xunit;

    public sealed class GameTests
    {
        [Fact]
        public void When_create_a_game_with_all_parameters_are_valid_must_be_created()
        {
            var name = "Name";

            var game = Game.Create(name);

            game.Should().NotBeNull();
        }

        [Fact]
        public void When_create_a_game_with_more_than_max_length_must_throw_an_exception()
        {
            var name = new string('c', Game.NAME_MAX_LENGTH + 1);

            Action action = () => Game.Create(name);

            action.Should().Throw<DomainException>()
                .WithMessage(DomainPreconditionMessages.GetLongerThan(Game.NAME_MAX_LENGTH, nameof(Game.Name)));
        }

        [Fact]
        public void When_create_a_game_without_name_must_throw_an_exception()
        {
            Action action = () => Game.Create(null);

            action.Should().Throw<DomainException>()
                .WithMessage(DomainPreconditionMessages.GetNotNull(nameof(Game.Name)));
        }
    }
}