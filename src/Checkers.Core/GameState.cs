namespace Checkers.Core;

public sealed class GameState
{
    public Board Board { get; }
    public PieceColor CurrentPlayer { get; private set; }
    public GameStatus Status { get; private set; }
    public int HalfMovesSinceCaptureOrKing { get; private set; }

    public GameState(Board board, PieceColor currentPlayer, GameStatus status = GameStatus.InProgress, int halfMovesSinceCaptureOrKing = 0)
    {
        Board = board ?? throw new ArgumentNullException(nameof(board));
        CurrentPlayer = currentPlayer;
        Status = status;
        HalfMovesSinceCaptureOrKing = halfMovesSinceCaptureOrKing;
    }

    public static GameState CreateStandard()
    {
        return new GameState(Board.CreateStandard(), PieceColor.Red, GameStatus.InProgress, 0);
    }

    public IReadOnlyList<Move> GetLegalMoves()
    {
        if (Status != GameStatus.InProgress)
        {
            return Array.Empty<Move>();
        }

        return MoveGenerator.GetLegalMoves(Board, CurrentPlayer);
    }

    public void ApplyMove(Move move)
    {
        if (Status != GameStatus.InProgress)
        {
            throw new InvalidOperationException("Game is already over.");
        }

        var legalMoves = GetLegalMoves();
        var resolved = ResolveMove(move, legalMoves);
        if (resolved == null)
        {
            throw new InvalidOperationException("Illegal move.");
        }

        var piece = Board.GetPiece(resolved.From);
        if (piece == null)
        {
            throw new InvalidOperationException("No piece at the starting position.");
        }

        Board.SetPiece(resolved.From, null);
        foreach (var capture in resolved.Captured)
        {
            Board.SetPiece(capture, null);
        }

        bool kinged = false;
        var movingPiece = piece.Value;
        if (!movingPiece.IsKing && IsBackRank(movingPiece.Color, resolved.To.Row))
        {
            movingPiece = movingPiece with { IsKing = true };
            kinged = true;
        }

        Board.SetPiece(resolved.To, movingPiece);

        if (resolved.IsCapture || kinged)
        {
            HalfMovesSinceCaptureOrKing = 0;
        }
        else
        {
            HalfMovesSinceCaptureOrKing++;
        }

        CurrentPlayer = CurrentPlayer == PieceColor.Red ? PieceColor.Black : PieceColor.Red;
        UpdateStatusAfterTurn();
    }

    private void UpdateStatusAfterTurn()
    {
        var opponent = CurrentPlayer;
        var opponentPieces = Board.CountPieces(opponent);
        if (opponentPieces == 0 || MoveGenerator.GetLegalMoves(Board, opponent).Count == 0)
        {
            Status = opponent == PieceColor.Red ? GameStatus.BlackWins : GameStatus.RedWins;
            return;
        }

        if (HalfMovesSinceCaptureOrKing >= 80)
        {
            Status = GameStatus.Draw;
            return;
        }

        Status = GameStatus.InProgress;
    }

    private static bool IsBackRank(PieceColor color, int row)
        => color == PieceColor.Red ? row == 0 : row == Board.Size - 1;

    private static Move? ResolveMove(Move input, IReadOnlyList<Move> legalMoves)
    {
        foreach (var legal in legalMoves)
        {
            if (legal.SequenceEquals(input))
            {
                return legal;
            }
        }

        foreach (var legal in legalMoves)
        {
            if (legal.PathMatches(input.Path))
            {
                return legal;
            }
        }

        return null;
    }
}
