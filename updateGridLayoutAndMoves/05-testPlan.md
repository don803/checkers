# Test plan

- ensure all existing tests pass before beginning work on code
- Alter test that must change as the associated tasks alter the code surface the tests cover
- Specify all new and altering tests below as well as the tasks they are acssociated with.
## Initial build spec review
- Reviewed `initialBuildDocumentation/06-specs.md` for unrecorded work.
- TP10 (draw detection) is marked incomplete there and remains out of scope because draw logic is not implemented in core.
- TP11–TP13 (CLI rendering/input/move application tests) are also incomplete there; no existing automated CLI test harness exists, so these are not required for this UI-only change (manual CLI validation will cover the flow).

## Out of scope
- TP10 (draw detection), TP11 (CLI render test), TP12 (CLI input parsing test), TP13 (CLI applies move test) are out of scope for this change set.

# Test plan for new UI tasks
- Build and run unit tests: `dotnet test` before and after UI changes to ensure core logic unchanged.
- Manual CLI validation of board rendering:
  - Verify top/bottom headers show `1 2 3 4 5 6 7 8`.
  - Verify left-side row labels show `a s d f g h j k` top-to-bottom.
  - Verify piece legend remains unchanged.
- Manual CLI validation of guided move flow:
  - At turn start, verify column choices match legal move starts; auto-select when only one.
  - After column selection, verify row choices limited to legal starts in that column; auto-select when only one.
  - After row selection, verify direction choices (1–4) reflect legal moves from that square; auto-select when only one.
  - If multiple destinations share a direction, verify disambiguation menu lists all destination squares sorted by column then row order.
  - Verify move summary format `numberLETTER->numberLETTER` and Enter executes move.
- Manual validation of backtracking:
  - From confirmation, `b` returns to direction selection (or prior visible step) and recomputes options.
  - `b` at direction selection returns to row selection; `b` at row selection returns to column selection; `b` at column selection stays.
  - Backtracking works even when prior steps were auto-selected.
- Manual validation of error handling:
  - Invalid column/row/direction/destination inputs show short, specific errors and re-prompt.
  - Invalid confirmation input (non-empty/non-b) re-prompts at confirmation.
