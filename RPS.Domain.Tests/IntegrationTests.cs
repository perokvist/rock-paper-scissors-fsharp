using Xunit;

namespace RPS.Domain.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void HandleCommand()
        {
            var cmd = new Game.CreateGameCommand("per", Game.Move.Paper, "TestGame", "Game001");
            GameHandler.handle(cmd);

            Assert.False(GameHandler.eventStore.IsEmpty);
        }
    }
}
