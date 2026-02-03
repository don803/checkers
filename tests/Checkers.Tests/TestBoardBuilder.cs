using Checkers.Core;

namespace Checkers.Tests;

internal static class TestBoardBuilder
{
    public static Board Empty() => new Board();

    public static Board WithPiece(PieceColor color, int row, int col, bool isKing = false)
    {
        var board = new Board();
        board.SetPiece(new Position(row, col), new Piece(color, isKing));
        return board;
    }

    public static void AddPiece(Board board, PieceColor color, int row, int col, bool isKing = false)
    {
        board.SetPiece(new Position(row, col), new Piece(color, isKing));
    }
}
