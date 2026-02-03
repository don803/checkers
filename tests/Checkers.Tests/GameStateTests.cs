using Checkers.Core;
using Xunit;

namespace Checkers.Tests;

public class GameStateTests
{
    [Fact]
    public void PieceIsKingedOnBackRank()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 1, 2);
        var game = new GameState(board, PieceColor.Red);

        var move = game.GetLegalMoves().Single(m => m.PathMatches(new[] { new Position(1, 2), new Position(0, 1) }));
        game.ApplyMove(move);

        var king = game.Board.GetPiece(new Position(0, 1));
        Assert.NotNull(king);
        Assert.True(king.Value.IsKing);
    }

    [Fact]
    public void WinIsDetectedWhenOpponentHasNoPieces()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 5, 2);
        var game = new GameState(board, PieceColor.Red);

        var move = game.GetLegalMoves().Single(m => m.PathMatches(new[] { new Position(5, 2), new Position(4, 1) }));
        game.ApplyMove(move);

        Assert.Equal(GameStatus.RedWins, game.Status);
    }

    [Fact]
    public void WinIsDetectedWhenOpponentHasNoLegalMoves()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 5, 2);
        TestBoardBuilder.AddPiece(board, PieceColor.Black, 7, 0);
        var game = new GameState(board, PieceColor.Red);

        var move = game.GetLegalMoves().Single(m => m.PathMatches(new[] { new Position(5, 2), new Position(4, 1) }));
        game.ApplyMove(move);

        Assert.Equal(GameStatus.RedWins, game.Status);
    }

    [Fact]
    public void DrawIsDetectedAfterEightyPlyWithoutCaptureOrKinging()
    {
        var board = new Board();
        TestBoardBuilder.AddPiece(board, PieceColor.Red, 3, 2, isKing: true);
        TestBoardBuilder.AddPiece(board, PieceColor.Black, 4, 5, isKing: true);

        var game = new GameState(board, PieceColor.Red, GameStatus.InProgress, halfMovesSinceCaptureOrKing: 79);
        var move = game.GetLegalMoves().Single(m => m.PathMatches(new[] { new Position(3, 2), new Position(4, 1) }));
        game.ApplyMove(move);

        Assert.Equal(GameStatus.Draw, game.Status);
    }
}
