using System.Linq;
using System.Security.AccessControl;
using Treefort.Commanding;
using Xunit;

namespace RPS.Domain.Tests
{
    public class IntegrationTests
    {
        private const string GameId = "Game001";

        public IntegrationTests()
        {
            var player1 = new Game.CreateGameCommand("per", Game.Move.Paper, "TestGame", GameId);
            var player2 = new Game.MakeMoveCommand(Game.Move.Scissors, "Christoffer", GameId);

            GameHandler.handle(player1);
            GameHandler.handle(player2);
        }

        [Fact]
        public void BestPersonWins()
        {
            var end = GameHandler.eventStore[GameId].OfType<Game.GameEndedEvent>().Single();
            Assert.True(end.result.IsPlayerTwoWin);
        }

        [Fact]
        public void AllEventsAreRecorded()
        {
            Assert.Equal(4, GameHandler.eventStore[GameId].Count());

            //TODO - read state / projection
            //GameHandler.handle(new Commands.MakeMoveCommand(Common.Move.Scissors, "olav", "a"));
        }
    }

}
