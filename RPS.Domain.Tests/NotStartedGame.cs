using System.Linq;
using Xunit;

namespace RPS.Domain.Tests
{
    public class NotStartedGame
    {
        private readonly Game.State _state;

        public NotStartedGame()
        {
            _state = new Game.State(Game.GameState.NotStarted, "creator", Game.Move.Rock);
        }

        [Fact]
        public void ShouldReturnCorrectEvents() // :P
        {
            var cmd = new Game.CreateGameCommand("per", Game.Move.Paper, "TestGame", "Game001");
            var events = Game.createGame(cmd, _state);

            Assert.True(events.OfType<Game.GameCreatedEvent>().Any());
            Assert.True(events.OfType<Game.MoveMadeEvent>().Any());
        }
    }
}
