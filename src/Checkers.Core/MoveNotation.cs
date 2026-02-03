namespace Checkers.Core;

public static class MoveNotation
{
    public static bool TryParsePath(string input, out IReadOnlyList<Position> path)
    {
        path = Array.Empty<Position>();
        if (string.IsNullOrWhiteSpace(input))
        {
            return false;
        }

        var normalized = input.Trim().Replace("x", "-").Replace("X", "-");
        var parts = normalized.Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        if (parts.Length < 2)
        {
            return false;
        }

        var positions = new List<Position>();
        foreach (var part in parts)
        {
            if (part.Length != 2)
            {
                return false;
            }

            char file = char.ToLowerInvariant(part[0]);
            char rankChar = part[1];

            if (file < 'a' || file > 'h')
            {
                return false;
            }

            if (rankChar < '1' || rankChar > '8')
            {
                return false;
            }

            int col = file - 'a';
            int rank = rankChar - '0';
            int row = Board.Size - rank;

            positions.Add(new Position(row, col));
        }

        path = positions;
        return true;
    }
}
