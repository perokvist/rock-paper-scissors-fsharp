using System;
using System.Linq;
using Xunit;

namespace RPS.Domain.Tests
{
    public class NotStartedGame
    {
        private readonly Game.State _state;

        public NotStartedGame()
        {
            _state = new Game.State(Game.GameState.NotStarted, "creator", Common.Move.Rock);
        }

        [Fact]
        public void ShouldReturnCorrectEvents() // :P
        {
            var cmd = new Commands.CreateGameCommand("per", Common.Move.Paper, "TestGame", Guid.NewGuid(), Guid.NewGuid());
            var events = Game.createGame(cmd, _state);

            Assert.True(events.OfType<Events.GameCreatedEvent>().Any());
            Assert.True(events.OfType<Events.MoveMadeEvent>().Any());
        }
    }
}
