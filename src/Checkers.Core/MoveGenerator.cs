namespace Checkers.Core;

public static class MoveGenerator
{
    private static readonly (int dr, int dc)[] RedDirections =
    {
        (-1, -1),
        (-1, 1)
    };

    private static readonly (int dr, int dc)[] BlackDirections =
    {
        (1, -1),
        (1, 1)
    };

    private static readonly (int dr, int dc)[] KingDirections =
    {
        (-1, -1),
        (-1, 1),
        (1, -1),
        (1, 1)
    };

    public static List<Move> GetLegalMoves(Board board, PieceColor player)
    {
        var captures = GetCaptureMoves(board, player);
        if (captures.Count > 0)
        {
            return captures;
        }

        return GetNonCaptureMoves(board, player);
    }

    public static List<Move> GetNonCaptureMoves(Board board, PieceColor player)
    {
        var moves = new List<Move>();
        foreach (var (position, piece) in board.GetPieces(player))
        {
            foreach (var (dr, dc) in GetDirections(piece))
            {
                var target = new Position(position.Row + dr, position.Col + dc);
                if (!board.IsInside(target) || board.GetPiece(target) != null)
                {
                    continue;
                }

                moves.Add(new Move(new[] { position, target }));
            }
        }
        return moves;
    }

    public static List<Move> GetCaptureMoves(Board board, PieceColor player)
    {
        var moves = new List<Move>();
        foreach (var (position, piece) in board.GetPieces(player))
        {
            moves.AddRange(GenerateCaptures(board, position, piece));
        }
        return moves;
    }

    private static IEnumerable<Move> GenerateCaptures(Board board, Position start, Piece piece)
    {
        var results = new List<Move>();
        var path = new List<Position> { start };
        var captured = new List<Position>();

        ExploreCaptures(board, start, piece, path, captured, results);
        return results;
    }

    private static void ExploreCaptures(
        Board board,
        Position current,
        Piece piece,
        List<Position> path,
        List<Position> captured,
        List<Move> results)
    {
        bool foundCapture = false;

        foreach (var (dr, dc) in GetDirections(piece))
        {
            var mid = new Position(current.Row + dr, current.Col + dc);
            var landing = new Position(current.Row + dr * 2, current.Col + dc * 2);

            if (!board.IsInside(mid) || !board.IsInside(landing))
            {
                continue;
            }

            var midPiece = board.GetPiece(mid);
            if (midPiece == null || midPiece.Value.Color == piece.Color)
            {
                continue;
            }

            if (board.GetPiece(landing) != null)
            {
                continue;
            }

            foundCapture = true;

            var nextBoard = board.Clone();
            nextBoard.SetPiece(current, null);
            nextBoard.SetPiece(mid, null);
            nextBoard.SetPiece(landing, piece);

            var nextPath = new List<Position>(path) { landing };
            var nextCaptured = new List<Position>(captured) { mid };

            ExploreCaptures(nextBoard, landing, piece, nextPath, nextCaptured, results);
        }

        if (!foundCapture && captured.Count > 0)
        {
            results.Add(new Move(path, captured));
        }
    }

    private static (int dr, int dc)[] GetDirections(Piece piece)
    {
        if (piece.IsKing)
        {
            return KingDirections;
        }

        return piece.Color == PieceColor.Red ? RedDirections : BlackDirections;
    }
}
