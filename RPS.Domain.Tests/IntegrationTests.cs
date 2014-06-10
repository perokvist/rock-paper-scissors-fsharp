using Xunit;

namespace RPS.Domain.Tests
{
    public class IntegrationTests
    {
        [Fact]
        public void HandleCommand()
        {
            //TODO - read state / projection
            GameHandler.handle(new Game.MakeMoveCommand(Game.Move.Scissors, "olav", "a"));
        }
    }
}
