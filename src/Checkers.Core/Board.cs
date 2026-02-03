namespace Checkers.Core;

public sealed class Board
{
    public const int Size = 8;
    private readonly Piece?[,] _squares = new Piece?[Size, Size];

    public Board()
    {
    }

    public static Board CreateStandard()
    {
        var board = new Board();
        board.InitializeStandardSetup();
        return board;
    }

    public void InitializeStandardSetup()
    {
        Array.Clear(_squares, 0, _squares.Length);

        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                var pos = new Position(row, col);
                if (!IsDarkSquare(pos))
                {
                    continue;
                }

                if (row <= 2)
                {
                    _squares[row, col] = new Piece(PieceColor.Black, false);
                }
                else if (row >= 5)
                {
                    _squares[row, col] = new Piece(PieceColor.Red, false);
                }
            }
        }
    }

    public Piece? GetPiece(Position position)
    {
        EnsureInside(position);
        return _squares[position.Row, position.Col];
    }

    public void SetPiece(Position position, Piece? piece)
    {
        EnsureInside(position);
        _squares[position.Row, position.Col] = piece;
    }

    public bool IsInside(Position position)
        => position.Row >= 0 && position.Row < Size && position.Col >= 0 && position.Col < Size;

    public bool IsDarkSquare(Position position) => (position.Row + position.Col) % 2 == 1;

    public IEnumerable<(Position Position, Piece Piece)> GetPieces(PieceColor? color = null)
    {
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                var piece = _squares[row, col];
                if (piece == null)
                {
                    continue;
                }

                if (color.HasValue && piece.Value.Color != color.Value)
                {
                    continue;
                }

                yield return (new Position(row, col), piece.Value);
            }
        }
    }

    public int CountPieces(PieceColor color)
        => GetPieces(color).Count();

    public Board Clone()
    {
        var clone = new Board();
        for (int row = 0; row < Size; row++)
        {
            for (int col = 0; col < Size; col++)
            {
                clone._squares[row, col] = _squares[row, col];
            }
        }
        return clone;
    }

    private void EnsureInside(Position position)
    {
        if (!IsInside(position))
        {
            throw new ArgumentOutOfRangeException(nameof(position), "Position is outside the board.");
        }
    }
}
