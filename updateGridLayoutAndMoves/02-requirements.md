# Requirements: New Terminal UI Interaction Model (C# / PowerShell)

## 1) Board Coordinate System and Display

### 1.1 Column labels (top and bottom)
- Replace the current `a b c d e f g h` column headers with **numeric labels**:
  - Columns are labeled **1 2 3 4 5 6 7 8** from left to right.
- These numeric headers appear at the **top and bottom** of the board.

### 1.2 Row labels (left side)
- Replace the current row numbers `8..1` shown on the left with **letter labels**.
- The left-side row labels must be:
  - `a` at the top row, then in order downward: `a s d f g h j k`
  - Mapping:
    - Top row label = `a`
    - Next row = `s`
    - Next row = `d`
    - Next row = `f`
    - Next row = `g`
    - Next row = `h`
    - Next row = `j`
    - Bottom row = `k`

### 1.3 Coordinate meaning
- A square is identified as: **columnNumber + rowLetter**
  - Example: `3f` means column 3, row `f`.
- The UI must treat these as the canonical coordinates when printing move choices.

### 1.4 Existing piece legend
- Keep existing piece symbols and legend text:
  - `b` = black man, `B` = black king, `r` = red man, `R` = red king
  - `.` = empty playable square
  - `=` = unplayable square

---

## 2) Turn Interaction Flow

At the start of **each player turn**, the UI must drive move selection through a guided decision tree.

### 2.1 Start-of-turn: show starting column options
- Compute all legal moves for the current player.
- Derive the set of **distinct starting columns** (1–8) that contain at least one legal move start square.
- Display those column numbers to the user (sorted ascending).
  - Example: `Choose a column: [2, 4, 5]`

### 2.2 Column selection input
- Prompt user for a single digit `1`–`8`.
- Input validation:
  - If input is not a valid option, show a brief error and re-prompt.
- If there is **only one** valid starting column, it must be **auto-selected** (no user input required), and the UI proceeds to the next step.

### 2.3 After column selected: show playable row letters
- Given the selected column, compute the set of **row letters** (`a s d f g h j k`) that are valid starting squares for a legal move in that column.
- Display those row letters to the user (in top-to-bottom board order).
  - Example: `Choose a row: [f, h]`
- If there is **only one** valid row letter, it must be **auto-selected**, establishing the selected piece square (the “from” square), then proceed to the next step.

---

## 3) Move Direction Selection

### 3.1 Direction menu
- Once the “from” square is determined (column + row letter), the UI must compute which **direction codes** are legal from that square, based on the current board state and move rules.
- Direction codes are fixed as:
  - `1` = up-left
  - `2` = up-right
  - `3` = down-right
  - `4` = down-left

### 3.2 Show only legal direction options
- Display only the direction numbers that correspond to legal moves from the selected piece.
  - Example: `Choose direction: [1, 2]`
- If there is **only one** legal direction, it must be **auto-selected** and the UI proceeds to confirmation.

### 3.3 Destination resolution
- Once direction is selected, the UI determines the destination square (including capture landing squares if applicable) according to the game’s legal-move generator.
- If multiple legal moves share the same direction from the same “from” square (e.g., different landing squares due to capture rules), the UI must present a disambiguation step:
  - Show each possible resulting `to` square and let user choose among them.
  - If only one destination exists, auto-select it.

---

## 4) Confirmation, Execution, and Backtracking

### 4.1 Move summary
- After a complete move is determined (`from` and `to`), display a single-line summary in this format:
  - `numberLETTER->numberLETTER`
  - Example: `3f->4g`

### 4.2 Confirmation input
- Prompt:
  - **Enter** (empty input) executes the move.
  - Typing `b` backs up the decision tree (see 4.3).
  - Any other input re-prompts at the confirmation step with a short error message.

### 4.3 Back behavior
- On `b` at the confirmation step:
  - Back up to the **previous decision point** (direction selection if direction was chosen; otherwise row selection; otherwise column selection).
- On `b` at any earlier step:
  - Back up one step.
- On `b` at the initial column selection step:
  - Remain at the initial column selection step (no further back).

### 4.4 Auto-selection and backtracking interaction
- If a step was auto-selected because only one option existed, and the user presses `b` later:
  - The UI still backs up to the previous *user-visible* decision level, and re-evaluates choices from there.

---

## 5) General UI Requirements

### 5.1 Determinism
- Option lists (columns, rows, directions, destinations) must be presented in a consistent, predictable order:
  - Columns: 1→8
  - Rows: `a s d f g h j k` (top→bottom)
  - Directions: 1→4
  - Destinations: sorted by (column asc, then row order top→bottom)

### 5.2 Recompute options dynamically
- At every decision step, options must be recomputed from the current board state (after moves, captures, kinging, etc.).

### 5.3 Error handling
- Invalid input must not crash the game.
- Errors should be short and specific (e.g., “Not an available column.”).

---

## 6) Non-Goals (explicitly out of scope for this UI change)
- No requirement to change game rules, legal move logic, forced capture rules, or multi-jump capture logic (unless already present).
- This change is strictly a **UI/interaction model** layered on top of existing move generation/apply logic.
