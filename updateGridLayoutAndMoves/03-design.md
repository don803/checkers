# Design Within Context

- Given the information you've collected thus far, generate a design that would achieve the requirements as stated in 02-requirements.md and exist without conflict given the things you learned in 02.5-CodebasedReconnaissance.md
- Record all aspects of your design plan below.
## Planned Design
- Scope changes to `src/Checkers.Cli/Program.cs` only; keep `Checkers.Core` move rules intact to avoid breaking tests and logic.
- Add a UI coordinate mapping layer in CLI:
  - Columns labeled `1..8` left-to-right map to `col = number - 1`.
  - Rows labeled `a s d f g h j k` top-to-bottom map to `row = index in ['a','s','d','f','g','h','j','k']`.
  - Provide helper to translate between `Position` and UI coordinate (string like `3f`).
- Update board rendering to show numeric column headers (top/bottom) and row letter labels on the left using the new row label order.
- Replace free-form path input with a guided decision tree:
  - Step 1: compute legal moves via `state.GetLegalMoves()` and derive valid starting columns; auto-select when only one.
  - Step 2: for chosen column, derive valid row letters (top-to-bottom order); auto-select when only one.
  - Step 3: from selected `from` square, derive legal direction codes (1..4) and prompt; auto-select when only one.
  - Step 4: if multiple destinations share that direction, show destination list sorted by column asc then row order; auto-select when only one.
  - Step 5: show move summary `numberLETTER->numberLETTER`, confirmation with Enter to execute, `b` to backtrack.
- Implement backtracking state in CLI (stack of decision levels) with recomputation at each step to honor dynamic board updates and forced captures.
- Use `Move` list as source of truth for filtering by column, row letter, direction, and destination; never regenerate rules in CLI.
- Keep piece legend text unchanged; only update coordinates shown to the player.
- Minimal helper methods added in `Program.cs` for:
  - building option sets (columns, rows, directions, destinations)
  - mapping between UI coordinates and `Position`
  - rendering prompts and validation
