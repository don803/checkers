namespace Checkers.Core;

public sealed class Move
{
    public IReadOnlyList<Position> Path { get; }
    public IReadOnlyList<Position> Captured { get; }

    public Position From => Path[0];
    public Position To => Path[^1];
    public bool IsCapture => Captured.Count > 0;

    public Move(IReadOnlyList<Position> path, IReadOnlyList<Position>? captured = null)
    {
        if (path == null || path.Count < 2)
        {
            throw new ArgumentException("Move path must contain at least a start and end position.", nameof(path));
        }

        Path = path.ToArray();
        Captured = captured == null ? Array.Empty<Position>() : captured.ToArray();
    }

    public bool PathMatches(IReadOnlyList<Position> other)
    {
        if (other == null || other.Count != Path.Count)
        {
            return false;
        }

        for (int i = 0; i < Path.Count; i++)
        {
            if (!Path[i].Equals(other[i]))
            {
                return false;
            }
        }

        return true;
    }

    public bool SequenceEquals(Move other)
    {
        if (!PathMatches(other.Path) || Captured.Count != other.Captured.Count)
        {
            return false;
        }

        for (int i = 0; i < Captured.Count; i++)
        {
            if (!Captured[i].Equals(other.Captured[i]))
            {
                return false;
            }
        }

        return true;
    }

    public override string ToString()
    {
        var path = string.Join("-", Path.Select(p => p.ToString()));
        return IsCapture ? $"{path} (x{Captured.Count})" : path;
    }
}
