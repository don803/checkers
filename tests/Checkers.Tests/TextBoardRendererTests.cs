using Checkers.Core;
using Xunit;

namespace Checkers.Tests;

public class TextBoardRendererTests
{
    [Fact]
    public void RenderIncludesCoordinates()
    {
        var board = Board.CreateStandard();
        var text = TextBoardRenderer.Render(board);

        Assert.Contains("a b c d e f g h", text);
        Assert.Contains(" 8 ", text);
        Assert.Contains(" 1 ", text);
    }
}
