namespace Checkers.Core;

public readonly record struct Piece(PieceColor Color, bool IsKing)
{
    public override string ToString() => IsKing ? $"{Color} King" : Color.ToString();
}
