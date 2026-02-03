namespace Checkers.Core;

public readonly struct Position : IEquatable<Position>
{
    public int Row { get; }
    public int Col { get; }

    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }

    public bool Equals(Position other) => Row == other.Row && Col == other.Col;

    public override bool Equals(object? obj) => obj is Position other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Row, Col);

    public override string ToString() => $"{Row},{Col}";
}
