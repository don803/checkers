using Checkers.Core;
using Xunit;

namespace Checkers.Tests;

public class MoveGenerationTests
{
    [Fact]
    public void StandardPieceGeneratesTwoForwardMoves()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 5, 2);

        var moves = MoveGenerator.GetNonCaptureMoves(board, PieceColor.Red);

        Assert.Contains(moves, move => move.PathMatches(new[] { new Position(5, 2), new Position(4, 1) }));
        Assert.Contains(moves, move => move.PathMatches(new[] { new Position(5, 2), new Position(4, 3) }));
        Assert.Equal(2, moves.Count);
    }

    [Fact]
    public void CaptureMoveIsGeneratedWhenJumpAvailable()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 5, 2);
        TestBoardBuilder.AddPiece(board, PieceColor.Black, 4, 3);

        var moves = MoveGenerator.GetCaptureMoves(board, PieceColor.Red);

        Assert.Single(moves);
        var move = moves[0];
        Assert.True(move.PathMatches(new[] { new Position(5, 2), new Position(3, 4) }));
        Assert.Equal(new[] { new Position(4, 3) }, move.Captured);
    }

    [Fact]
    public void MandatoryCaptureExcludesNonCaptures()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 5, 2);
        TestBoardBuilder.AddPiece(board, PieceColor.Black, 4, 3);
        TestBoardBuilder.AddPiece(board, PieceColor.Red, 5, 6);

        var moves = MoveGenerator.GetLegalMoves(board, PieceColor.Red);

        Assert.All(moves, move => Assert.True(move.IsCapture));
    }

    [Fact]
    public void MultiJumpCaptureIsGenerated()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 5, 0);
        TestBoardBuilder.AddPiece(board, PieceColor.Black, 4, 1);
        TestBoardBuilder.AddPiece(board, PieceColor.Black, 2, 3);

        var moves = MoveGenerator.GetCaptureMoves(board, PieceColor.Red);

        Assert.Contains(moves, move => move.PathMatches(new[]
        {
            new Position(5, 0),
            new Position(3, 2),
            new Position(1, 4)
        }));
    }

    [Fact]
    public void KingCanMoveAndCaptureBackward()
    {
        var board = TestBoardBuilder.WithPiece(PieceColor.Red, 3, 2, isKing: true);
        var moves = MoveGenerator.GetNonCaptureMoves(board, PieceColor.Red);

        Assert.Contains(moves, move => move.PathMatches(new[] { new Position(3, 2), new Position(4, 1) }));
        Assert.Contains(moves, move => move.PathMatches(new[] { new Position(3, 2), new Position(4, 3) }));

        var captureBoard = TestBoardBuilder.WithPiece(PieceColor.Red, 3, 2, isKing: true);
        TestBoardBuilder.AddPiece(captureBoard, PieceColor.Black, 4, 3);

        var captureMoves = MoveGenerator.GetCaptureMoves(captureBoard, PieceColor.Red);
        Assert.Contains(captureMoves, move => move.PathMatches(new[] { new Position(3, 2), new Position(5, 4) }));
    }
}
