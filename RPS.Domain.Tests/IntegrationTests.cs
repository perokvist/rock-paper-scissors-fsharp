using System;
using System.Linq;
using System.Security.AccessControl;
using Treefort.Commanding;
using Xunit;

namespace RPS.Domain.Tests
{
    public class IntegrationTests
    {
        private readonly Guid _gameId = Guid.NewGuid();

        public IntegrationTests()
        {
            var correlationId = Guid.NewGuid();
            var player1 = new Commands.CreateGameCommand("per", Common.Move.Paper, "TestGame", _gameId, correlationId);
            var player2 = new Commands.MakeMoveCommand(Common.Move.Scissors, "Christoffer", _gameId, correlationId);

            GameHandler.handle(player1);
            GameHandler.handle(player2);
        }

        [Fact]
        public void BestPersonWins()
        {
            var end = GameHandler.eventStore[_gameId].OfType<Events.GameEndedEvent>().Single();
            Assert.True(end.result.IsPlayerTwoWin);
        }

        [Fact]
        public void AllEventsAreRecorded()
        {
            Assert.Equal(4, GameHandler.eventStore[_gameId].Count());

            //TODO - read state / projection
            //GameHandler.handle(new Commands.MakeMoveCommand(Common.Move.Scissors, "olav", "a"));
        }
    }

}
