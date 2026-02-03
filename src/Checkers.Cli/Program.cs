using Checkers.Core;

Console.WriteLine("Checkers - enter moves like b6-a5 or b6-d4-f2. Type 'q' to quit.");

var game = GameState.CreateStandard();

while (true)
{
    Console.WriteLine();
    Console.WriteLine(TextBoardRenderer.Render(game.Board));

    if (game.Status != GameStatus.InProgress)
    {
        switch (game.Status)
        {
            case GameStatus.RedWins:
                Console.WriteLine("Red wins.");
                break;
            case GameStatus.BlackWins:
                Console.WriteLine("Black wins.");
                break;
            case GameStatus.Draw:
                Console.WriteLine("Game is a draw.");
                break;
        }
        break;
    }

    Console.Write($"{game.CurrentPlayer} to move > ");
    var input = Console.ReadLine();
    if (input == null)
    {
        continue;
    }

    input = input.Trim();
    if (input.Equals("q", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (!MoveNotation.TryParsePath(input, out var path))
    {
        Console.WriteLine("Invalid input. Use format like b6-a5 or b6-d4-f2.");
        continue;
    }

    var legalMoves = game.GetLegalMoves();
    var selected = legalMoves.FirstOrDefault(m => m.PathMatches(path));
    if (selected == null)
    {
        Console.WriteLine("Illegal move.");
        continue;
    }

    game.ApplyMove(selected);
}
