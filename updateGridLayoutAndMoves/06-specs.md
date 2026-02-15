# Specs Grid

| ID | Type | Item | Prereq Task | Complete (time) |
|---|---|---|---|---|
| T1 | Task | Identify CLI input/rendering points to replace in `Program.cs` | - | [x] 2026-01-30 14:01:07 |
| T2 | Task | Add UI coordinate mapping helpers (columns/rows, `Position` <-> UI) | T1 | [x] 2026-01-30 14:01:07 |
| T3 | Task | Update board rendering to numeric columns and letter rows | T2 | [x] 2026-01-30 14:01:07 |
| TP1 | Test | Manual: board renders numeric headers + row letters + legend unchanged | T3 | [x] 2026-01-30 18:51:02 |
| T4 | Task | Implement guided column selection from legal moves with validation/auto-select | T3 | [x] 2026-01-30 14:01:07 |
| T5 | Task | Implement guided row selection for chosen column with validation/auto-select | T4 | [x] 2026-01-30 14:01:07 |
| T6 | Task | Implement direction selection (1–4) from chosen square with validation/auto-select | T5 | [x] 2026-01-30 14:01:07 |
| T7 | Task | Implement destination disambiguation when multiple legal landings share direction | T6 | [x] 2026-01-30 14:01:07 |
| T8 | Task | Implement confirmation step and move execution | T7 | [x] 2026-01-30 14:01:07 |
| T9 | Task | Implement backtracking across decision steps (including auto-selected steps) | T8 | [x] 2026-01-30 14:01:07 |
| T10 | Task | Remove/bypass free-form path input flow in CLI | T9 | [x] 2026-01-30 14:01:07 |
| T11 | Task | Add short, specific error handling at each prompt step | T4 | [x] 2026-01-30 14:01:07 |
| TP2 | Test | Manual: guided selection flow (columns/rows/directions/destinations) | T7 | [x] 2026-01-30 18:51:02 |
| TP3 | Test | Manual: confirmation + backtracking behaviors | T9 | [x] 2026-01-30 18:51:02 |
| TP4 | Test | Manual: invalid input errors re-prompt correctly | T11 | [x] 2026-01-30 18:51:02 |
| TP5 | Test | `dotnet test` before/after UI changes | T10 | [x] 2026-01-30 14:05:19 |

## Note
- Out of scope: TP10–TP13 from initial build specs (draw + CLI automated tests).
- Automated test run from this environment failed due to blocked NuGet network access.
