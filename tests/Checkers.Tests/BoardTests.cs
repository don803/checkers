using Checkers.Core;
using Xunit;

namespace Checkers.Tests;

public class BoardTests
{
    [Fact]
    public void StandardSetupHasTwelvePiecesPerSideOnDarkSquares()
    {
        var board = Board.CreateStandard();

        Assert.Equal(12, board.CountPieces(PieceColor.Red));
        Assert.Equal(12, board.CountPieces(PieceColor.Black));

        foreach (var (position, _) in board.GetPieces())
        {
            Assert.True(board.IsDarkSquare(position));
        }
    }
}
