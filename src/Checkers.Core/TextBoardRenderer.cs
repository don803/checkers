using System.Text;

namespace Checkers.Core;

public static class TextBoardRenderer
{
    public static string Render(Board board)
    {
        var sb = new StringBuilder();
        sb.AppendLine("    a b c d e f g h");

        for (int row = 0; row < Board.Size; row++)
        {
            int rank = Board.Size - row;
            sb.Append($" {rank} ");

            for (int col = 0; col < Board.Size; col++)
            {
                var pos = new Position(row, col);
                var piece = board.GetPiece(pos);
                char symbol;

                if (piece == null)
                {
                    symbol = board.IsDarkSquare(pos) ? '.' : ' ';
                }
                else
                {
                    symbol = piece.Value.Color switch
                    {
                        PieceColor.Red => piece.Value.IsKing ? 'R' : 'r',
                        PieceColor.Black => piece.Value.IsKing ? 'B' : 'b',
                        _ => '?'
                    };
                }

                sb.Append(symbol);
                if (col < Board.Size - 1)
                {
                    sb.Append(' ');
                }
            }

            sb.Append($" {rank}");
            sb.AppendLine();
        }

        sb.AppendLine("    a b c d e f g h");
        return sb.ToString();
    }
}
