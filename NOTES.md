# Learning Notes

Read this on your phone between laptop sessions. Each day's section fills in after that day's branch
is merged to `main` — recap first, then flashcard-style Q&A for self-testing without a laptop.

## Roadmap

| Day | Branch | Topic | Status |
|-----|--------|-------|--------|
| 1 | `day1-wpf-fundamentals` | WPF basics: XAML, layout panels, controls, Dependency Properties | done |
| 2 | `day2-mvvm-databinding` | MVVM, data binding, commands, validation | not started |
| 3 | `day3-db-rest-integration` | DB (SQLite/EF Core) + REST calls + Dispatcher/threading | not started |
| 4 | `day4-winforms-and-wcf` | Windows Forms contrast + minimal WCF service/client | not started |
| 5 | `day5-interview-prep` | Mock Q&A, JD talking points (kiosks, peripherals, security) | not started |

---

## Day 1 — WPF Fundamentals

**Project**: `src/Wpf.CustomerManager` — a "New Customer" entry form. No data binding yet (that's
Day 2) — every field is read directly off the control in code-behind, the same way you'd do it in
Windows Forms. That's intentional: Day 2 rewrites `Save_Click` as a bound `ICommand` so you can feel
the actual contrast instead of just being told about it.

**How to run it** (laptop only — this is a native Windows GUI, nothing to view on phone):
```
dotnet run --project src/Wpf.CustomerManager
```
or open `WPF-Learning.sln` in Visual Studio and press F5.

**What it covers**:
- **Layout**: an outer `Grid` with `RowDefinitions` (`Auto`/`*`) for header/form/buttons/status, and
  a nested `Grid` with two `ColumnDefinitions` (label column `Auto`, input column `*`) for the form
  itself. `StackPanel Orientation="Horizontal"` for the button row.
- **Controls**: `TextBox`, `DatePicker`, `ComboBox`/`ComboBoxItem`, `Label`, `Border` (used here just
  as a styled status box, not for a border effect specifically).
- **Dependency Properties**: `Styles.xaml` sets `Width`/`Margin`/`Background`/etc. via `<Style
  TargetType="...">` — this only works because those are DPs, not plain CLR properties. A style is a
  batch of DP value assignments applied through WPF's property system.
- **Implicit vs keyed styles**: `TargetType="TextBox"` with no `x:Key` applies to every `TextBox` in
  scope automatically; `x:Key="PrimaryButton"` is opt-in, applied via `Style="{StaticResource
  PrimaryButton}"`. `SecondaryButton` uses `BasedOn="{StaticResource PrimaryButton}"` to inherit and
  override.
- **ResourceDictionary merging**: `Styles.xaml` is a standalone dictionary, pulled into the app via
  `Application.Resources` → `MergedDictionaries` in `App.xaml` — so it applies everywhere, not just
  one window.
- **Routed events**: `Click="Save_Click"` is a `RoutedEvent` (bubbles up the visual tree) — different
  from a plain CLR `event`, even though the code-behind syntax looks identical to a WinForms handler.

### Flashcards (phone review — no laptop needed)

1. **Q**: What is a Dependency Property, and why does WPF use it instead of a plain C# auto-property?
   **A**: A DP is a property registered with WPF's property system (`DependencyProperty.Register`)
   that gets styling, data binding, animation, and change notification for free — a plain CLR property
   can't be targeted by a `<Style>` setter or animated, because there's no hook for WPF to intervene
   when the value changes.
2. **Q**: Difference between an implicit style and a keyed style?
   **A**: Implicit = `TargetType` only, no `x:Key` → auto-applies to every control of that type in
   scope. Keyed = has `x:Key` → only applied where you explicitly reference it via
   `{StaticResource Key}`.
3. **Q**: What does `BasedOn="{StaticResource X}"` do?
   **A**: Inherits all setters from style `X`, then lets you add/override specific ones — like a
   subclass for styles.
4. **Q**: Why `RowDefinition Height="Auto"` vs `"*"` vs a fixed number?
   **A**: `Auto` = size to content; `*` = take remaining space (proportionally, if multiple `*` rows);
   fixed = exact pixel size regardless of content. Mixing `Auto` for header/footer rows and `*` for
   the body is the standard WPF layout idiom — avoid fixed heights, they don't adapt to content or
   window resizing.
5. **Q**: Is `Button.Click` a plain CLR event or a routed event? Why does it matter?
   **A**: Routed event — it bubbles up the visual tree, so a handler on a parent container (e.g. the
   `Window` itself) could also observe clicks from any child button without each button needing its
   own wired-up handler. Plain CLR events don't do this.
6. **Q**: What's the WinForms-equivalent of what `Save_Click` is doing here?
   **A**: Exactly the WinForms pattern — read control values directly via their field/`x:Name`
   reference inside the event handler, no binding layer in between. Day 2 replaces this with MVVM.

### Day 1 — Fundamentals Reference (how to build this from a blank project)

**Creating the project in Visual Studio**: File → New Project → "WPF Application" (C#) → name it,
pick `.NET 8.0 (Long-Term Support)` as the framework → Create. That generates `App.xaml`/`.cs`,
`MainWindow.xaml`/`.cs`, `AssemblyInfo.cs`, and the `.csproj` — exactly what's in
`src/Wpf.CustomerManager`. To add a second project to the same solution later (Day 4): right-click
the **Solution** in Solution Explorer → Add → New Project.

**Solution (`.slnx`) vs project (`.csproj`)**: the solution is just a list of which projects open
together — you basically never hand-edit it. The `.csproj` is where real config lives:
`<TargetFramework>net8.0-windows</TargetFramework>` (the `-windows` suffix unlocks WPF/WinForms
APIs — plain `net8.0` can't see them), `<UseWPF>true</UseWPF>` (the one WPF-specific switch; its
WinForms equivalent is `<UseWindowsForms>true</UseWindowsForms>`), `<OutputType>WinExe</OutputType>`
(GUI exe, no console window).

**How XAML connects to code-behind**: `x:Class="Wpf.CustomerManager.MainWindow"` in the `.xaml` and
`partial class MainWindow : Window` in the `.xaml.cs` are the *same class*, split across two files
via `partial`. The generated half (hidden in `obj/`) contains `InitializeComponent()`, which creates
every control you declared and assigns it to a field named by its `x:Name` — that's why
`txtFirstName` etc. just exist as usable variables with no manual wiring.

**XAML's core rule**: an element = "construct an instance of this class," an attribute = "set this
property on it." `<TextBox Width="220" />` ≡ `new TextBox { Width = 220 }`. The `xmlns` lines are
`using` statements: no-prefix = standard WPF controls, `x:` = the XAML language itself (`x:Class`,
`x:Name`, `x:Key`), `local:` = your own C# namespace.

**Window dimensions**: `Height`/`Width` are plain properties. `WindowStartupLocation` =
`Manual` (default) / `CenterScreen` / `CenterOwner` (for dialogs). `ResizeMode` = `CanResize`
(default) / `NoResize` / `CanMinimize` / `CanResizeWithGrip`.

**Grid**: rows × columns. `RowDefinition Height="Auto"` (size to content) / `"*"` (share remaining
space) / a number (fixed pixels — avoid, doesn't adapt). Children declare their cell via
`Grid.Row="0" Grid.Column="1"` — written on the *child*, not the `Grid`. This is an **attached
property**: a property `Grid` defines that any child placed inside it can carry, because the parent
needs to read "where do you belong" off each child.

**StackPanel**: no grid, just lines children up in order — vertical by default, `Orientation="Horizontal"`
for a row (used here for the Save/Clear buttons). `DockPanel` (dock children Top/Bottom/Left/Right,
one fills remaining space) and `Canvas` (absolute X/Y) exist too — recognize them by name for an
interview, no hands-on time needed yet.

**Controls used so far**:
- `Label Content="..."` — caption next to an input (`Content` can hold any object; `TextBlock Text="..."`
  is the plain-string-only equivalent, used for the page header).
- `TextBox` — free text; `Text="..."` to pre-fill, `TextWrapping="Wrap" AcceptsReturn="True"` + real
  height for multi-line.
- `DatePicker` — the calendar dropdown is automatic, nothing to configure to "turn it on." Read the
  value via `.SelectedDate` (`DateTime?`, nullable).
- `ComboBox` + `ComboBoxItem Content="..."` — hardcoded options directly in markup, fine for a small
  fixed list. `SelectedIndex="0"` sets the default. A list sourced from code/DB instead uses
  `ItemsSource="{Binding ...}"` — that's Day 2.

**Visual Studio's designer isn't a separate skill** — dragging a control from the Toolbox
(`Ctrl+Alt+X`) onto the design surface, or setting a property via the Properties window (`F4`),
writes the exact same XAML attributes shown above. Typing XAML directly is just faster once this
vocabulary is familiar.

**Self-check exercise before Day 2**: by hand, add a `CheckBox` ("Subscribe to newsletter") and a
multi-line `TextBox` ("Notes") to the form — new `RowDefinition`s, correct `Grid.Row` indices, read
`.IsChecked`/`.Text` in `Save_Click`. Do it once by typing XAML, once by dragging from the Toolbox,
confirm they produce the same markup.
