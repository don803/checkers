using System.Collections.Generic;
using System.Linq;
using Checkers.Core;
using Xunit;

namespace Checkers.Tests;

public class GameRulesTests
{
    [Fact]
    public void BoardInitializesWithStandardLayout()
    {
        var board = Board.CreateStandard();

        Assert.Equal(12, board.CountPieces(PlayerColor.Red));
        Assert.Equal(12, board.CountPieces(PlayerColor.Black));

        foreach (var (pos, _) in board.EnumeratePieces(PlayerColor.Red))
        {
            Assert.True(pos.IsDarkSquare);
        }

        foreach (var (pos, _) in board.EnumeratePieces(PlayerColor.Black))
        {
            Assert.True(pos.IsDarkSquare);
        }
    }

    [Fact]
    public void StandardPieceGeneratesNonCaptureMoves()
    {
        var board = Board.CreateEmpty();
        var start = new Position(5, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        var targets = moves.Select(m => m.To).ToList();
        Assert.Contains(new Position(4, 1), targets);
        Assert.Contains(new Position(4, 3), targets);
    }

    [Fact]
    public void CaptureMoveGeneratedWhenOpponentAdjacent()
    {
        var board = Board.CreateEmpty();
        var start = new Position(5, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red));
        board.SetPiece(new Position(4, 3), new Piece(PlayerColor.Black));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        Assert.Single(moves);
        Assert.Equal(new Position(3, 4), moves[0].To);
        Assert.True(moves[0].IsCapture);
    }

    [Fact]
    public void MandatoryCaptureExcludesNonCaptures()
    {
        var board = Board.CreateEmpty();
        var capturePiece = new Position(5, 2);
        var normalPiece = new Position(5, 4);
        board.SetPiece(capturePiece, new Piece(PlayerColor.Red));
        board.SetPiece(normalPiece, new Piece(PlayerColor.Red));
        board.SetPiece(new Position(4, 3), new Piece(PlayerColor.Black));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        Assert.All(moves, move => Assert.True(move.IsCapture));
    }

    [Fact]
    public void MultiJumpCaptureSequenceGenerated()
    {
        var board = Board.CreateEmpty();
        var start = new Position(5, 0);
        board.SetPiece(start, new Piece(PlayerColor.Red));
        board.SetPiece(new Position(4, 1), new Piece(PlayerColor.Black));
        board.SetPiece(new Position(2, 3), new Piece(PlayerColor.Black));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);
        var move = moves.Single();

        Assert.Equal(new Position(1, 4), move.To);
        Assert.Equal(2, move.Captured.Count);
        Assert.Equal(3, move.Path.Count);
    }

    [Fact]
    public void PieceBecomesKingOnBackRow()
    {
        var board = Board.CreateEmpty();
        var start = new Position(1, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red));

        var state = new GameState(board, PlayerColor.Red);
        var moves = state.GetLegalMoves();
        var move = moves.Single(m => m.To == new Position(0, 1));

        state.ApplyMove(move);

        var piece = board.GetPiece(new Position(0, 1));
        Assert.NotNull(piece);
        Assert.True(piece!.IsKing);
    }

    [Fact]
    public void KingMovesBackward()
    {
        var board = Board.CreateEmpty();
        var start = new Position(3, 2);
        board.SetPiece(start, new Piece(PlayerColor.Red, isKing: true));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);
        var targets = moves.Select(m => m.To).ToList();

        Assert.Contains(new Position(2, 1), targets);
        Assert.Contains(new Position(2, 3), targets);
        Assert.Contains(new Position(4, 1), targets);
        Assert.Contains(new Position(4, 3), targets);
    }

    [Fact]
    public void WinDetectedWhenOpponentHasNoPieces()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));
        var state = new GameState(board, PlayerColor.Black);

        state.UpdateStatus();

        Assert.Equal(GameStatus.RedWins, state.Status);
    }

    [Fact]
    public void WinDetectedWhenOpponentHasNoLegalMoves()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(0, 1), new Piece(PlayerColor.Red));
        board.SetPiece(new Position(7, 0), new Piece(PlayerColor.Black));

        var state = new GameState(board, PlayerColor.Black);
        state.UpdateStatus();

        Assert.Equal(GameStatus.RedWins, state.Status);
    }

    // ========== Position.TryParse Tests ==========

    [Theory]
    [InlineData("a1", 7, 0)]
    [InlineData("h8", 0, 7)]
    [InlineData("d4", 4, 3)]
    [InlineData("A1", 7, 0)]  // Case insensitive
    [InlineData("H8", 0, 7)]
    public void PositionTryParse_ValidInput_ReturnsTrue(string input, int expectedRow, int expectedCol)
    {
        var result = Position.TryParse(input, out var position);

        Assert.True(result);
        Assert.Equal(expectedRow, position.Row);
        Assert.Equal(expectedCol, position.Col);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("a")]
    [InlineData("1")]
    [InlineData("a0")]
    [InlineData("a9")]
    [InlineData("i1")]
    [InlineData("z5")]
    [InlineData("aa1")]
    [InlineData("1a")]
    public void PositionTryParse_InvalidInput_ReturnsFalse(string input)
    {
        var result = Position.TryParse(input, out _);

        Assert.False(result);
    }

    [Fact]
    public void PositionTryParse_NullInput_ReturnsFalse()
    {
        var result = Position.TryParse(null!, out _);

        Assert.False(result);
    }

    [Theory]
    [InlineData(0, 0, "a8")]
    [InlineData(7, 7, "h1")]
    [InlineData(4, 3, "d4")]
    public void PositionToNotation_ValidPosition_ReturnsCorrectNotation(int row, int col, string expected)
    {
        var position = new Position(row, col);

        Assert.Equal(expected, position.ToNotation());
    }

    [Fact]
    public void PositionToNotation_InvalidPosition_ReturnsQuestionMark()
    {
        var position = new Position(-1, -1);

        Assert.Equal("?", position.ToNotation());
    }

    // ========== TryApplyMove Error Cases ==========

    [Fact]
    public void TryApplyMove_IllegalMove_ReturnsFalseWithError()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));
        var state = new GameState(board, PlayerColor.Red);

        // Try to move to an invalid position (not diagonal, too far)
        var illegalPath = new List<Position> { new Position(5, 2), new Position(3, 2) };
        var result = state.TryApplyMove(illegalPath, out var error);

        Assert.False(result);
        Assert.Equal("Illegal move.", error);
    }

    [Fact]
    public void TryApplyMove_GameOver_ReturnsFalseWithError()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));
        var state = new GameState(board, PlayerColor.Black);
        state.UpdateStatus(); // This should set RedWins since Black has no pieces

        var path = new List<Position> { new Position(5, 2), new Position(4, 1) };
        var result = state.TryApplyMove(path, out var error);

        Assert.False(result);
        Assert.Equal("Game is over.", error);
    }

    [Fact]
    public void TryApplyMove_ValidMove_ReturnsTrueAndAppliesMove()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));
        board.SetPiece(new Position(2, 5), new Piece(PlayerColor.Black));
        var state = new GameState(board, PlayerColor.Red);

        var path = new List<Position> { new Position(5, 2), new Position(4, 1) };
        var result = state.TryApplyMove(path, out var error);

        Assert.True(result);
        Assert.Equal(string.Empty, error);
        Assert.Null(board.GetPiece(new Position(5, 2)));
        Assert.NotNull(board.GetPiece(new Position(4, 1)));
    }

    // ========== Board Edge Cases ==========

    [Fact]
    public void BoardSetPiece_InvalidPosition_ThrowsException()
    {
        var board = Board.CreateEmpty();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            board.SetPiece(new Position(-1, 0), new Piece(PlayerColor.Red)));
    }

    [Fact]
    public void BoardGetPiece_InvalidPosition_ReturnsNull()
    {
        var board = Board.CreateEmpty();

        var piece = board.GetPiece(new Position(-1, 0));

        Assert.Null(piece);
    }

    [Fact]
    public void BoardClone_CreatesIndependentCopy()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));

        var clone = board.Clone();
        board.SetPiece(new Position(5, 2), null);

        Assert.Null(board.GetPiece(new Position(5, 2)));
        Assert.NotNull(clone.GetPiece(new Position(5, 2)));
    }

    // ========== Player Switching ==========

    [Fact]
    public void ApplyMove_SwitchesCurrentPlayer()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));
        board.SetPiece(new Position(2, 5), new Piece(PlayerColor.Black));
        var state = new GameState(board, PlayerColor.Red);

        var moves = state.GetLegalMoves();
        state.ApplyMove(moves[0]);

        Assert.Equal(PlayerColor.Black, state.CurrentPlayer);
    }

    [Fact]
    public void ApplyMove_BlackToRed_SwitchesCorrectly()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(5, 2), new Piece(PlayerColor.Red));
        board.SetPiece(new Position(2, 5), new Piece(PlayerColor.Black));
        var state = new GameState(board, PlayerColor.Black);

        var moves = state.GetLegalMoves();
        state.ApplyMove(moves[0]);

        Assert.Equal(PlayerColor.Red, state.CurrentPlayer);
    }

    // ========== Piece at Board Corners/Edges ==========

    [Fact]
    public void PieceAtCorner_GeneratesLimitedMoves()
    {
        var board = Board.CreateEmpty();
        // Place red piece at bottom-left corner (row 7, col 0)
        board.SetPiece(new Position(7, 0), new Piece(PlayerColor.Red));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        // From corner, only one diagonal move is possible
        Assert.Single(moves);
        Assert.Equal(new Position(6, 1), moves[0].To);
    }

    [Fact]
    public void PieceAtEdge_GeneratesLimitedMoves()
    {
        var board = Board.CreateEmpty();
        // Place red piece at left edge (row 5, col 0)
        board.SetPiece(new Position(5, 0), new Piece(PlayerColor.Red));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        // From left edge, only one forward diagonal is valid
        Assert.Single(moves);
        Assert.Equal(new Position(4, 1), moves[0].To);
    }

    // ========== Multi-Jump Termination on King Promotion ==========

    [Fact]
    public void MultiJump_StopsOnKingPromotion()
    {
        var board = Board.CreateEmpty();
        // Red piece at row 2, can capture to row 0 (back row) but there's another piece to capture after
        board.SetPiece(new Position(2, 1), new Piece(PlayerColor.Red));
        board.SetPiece(new Position(1, 2), new Piece(PlayerColor.Black));
        // This piece would be capturable if multi-jump continued, but shouldn't be
        // because piece becomes king on reaching row 0

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        // Should have exactly one capture move that ends at row 0
        Assert.Single(moves);
        Assert.Equal(new Position(0, 3), moves[0].To);
        Assert.Single(moves[0].Captured);
    }

    // ========== Game Status Remains InProgress ==========

    [Fact]
    public void UpdateStatus_BothPlayersHaveMoves_RemainsInProgress()
    {
        var board = Board.CreateStandard();
        var state = new GameState(board, PlayerColor.Red);

        state.UpdateStatus();

        Assert.Equal(GameStatus.InProgress, state.Status);
    }

    // ========== King Captures Backward ==========

    [Fact]
    public void KingCapturesBackward()
    {
        var board = Board.CreateEmpty();
        board.SetPiece(new Position(3, 2), new Piece(PlayerColor.Red, isKing: true));
        board.SetPiece(new Position(4, 3), new Piece(PlayerColor.Black));

        var moves = MoveGenerator.GenerateLegalMoves(board, PlayerColor.Red);

        Assert.Single(moves);
        Assert.True(moves[0].IsCapture);
        Assert.Equal(new Position(5, 4), moves[0].To);
    }
}
