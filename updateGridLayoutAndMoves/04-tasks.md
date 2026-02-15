# Tasks
1. Inspect `src/Checkers.Cli/Program.cs` and identify the current move-input flow and rendering points to replace.
2. Add UI coordinate mapping helpers in `Program.cs` for column/row labels and conversions between `Position` and UI notation (e.g., `3f`).
3. Update board rendering to use numeric column headers and left-side row letters (`a s d f g h j k`) while preserving piece symbols/legend.
4. Implement guided move selection: compute legal moves, derive valid starting columns, prompt/auto-select, validate input.
5. Extend selection to row letters for the chosen column, prompt/auto-select, validate input.
6. Compute legal direction codes (1–4) from the selected square based on available legal moves, prompt/auto-select, validate input.
7. If multiple destinations share the same direction, present a destination disambiguation menu sorted by column then row order.
8. Add confirmation step showing `numberLETTER->numberLETTER`, support Enter to execute and `b` to backtrack to the previous decision point.
9. Implement backtracking logic so `b` navigates up the decision tree (respecting auto-selection rules) without leaving the current turn.
10. Remove or bypass free-form path parsing in CLI; keep core parsing for internal use if needed.
11. Ensure invalid inputs show short, specific errors and re-prompt at the correct step.
12. Quick manual sanity run in CLI (build/run) to validate the flow and rendering.
