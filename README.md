# WPF-Learning

Interview-prep practice repo: WPF, Windows Forms, and WCF, built up day by day ahead of a job-switch
interview. See [NOTES.md](NOTES.md) for the day-by-day roadmap, what each day covers, and
flashcard-style Q&A for phone review between laptop sessions.

## Workflow

`main` only ever holds **finished, understood** work. Each day/feature is built on its own branch and
merged into `main` once the concept has actually clicked — not just once the code compiles. Branch
naming: `dayN-topic` (e.g. `day1-wpf-fundamentals`, `day2-mvvm-databinding`).

## Structure

```
src/
  Wpf.CustomerManager/       # evolving WPF app (Days 1-3): layout -> MVVM/binding -> DB+REST
  WinForms.CustomerManager/  # Day 4: the same scenario in Windows Forms, for contrast
  Wcf.DemoService/           # Day 4: minimal WCF service
  Wcf.DemoClient/            # Day 4: console client consuming it
NOTES.md                    # day-by-day recap + flashcards (phone-readable)
```
