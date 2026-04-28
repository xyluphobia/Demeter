using Godot;
using System;

public partial class Sprint1Debug : Node
{
  Label dayLabel;
  Label seasonLabel;
  Label tapPosLabel;
  Label draggingLabel;

	public override void _Ready()
	{
    Button progressDayButton = GetNode<Button>("Canvas/Skip/ProgressDay");
    Button progressSeasonButton = GetNode<Button>("Canvas/Skip/ProgressSeason");
    progressDayButton.Pressed += ProgressDay;
    progressSeasonButton.Pressed += ProgressSeason;

    GameCalendar.I.DayPassed += OnDayPassed;
    GameCalendar.I.SeasonChanged += OnDayPassed;
    InputManager.I.Tapped += OnTapped;
    InputManager.I.Dragging += OnDragging;

    dayLabel = GetNode<Label>("Canvas/Outputs/Day/Result");
    seasonLabel = GetNode<Label>("Canvas/Outputs/Season/Result");
    tapPosLabel = GetNode<Label>("Canvas/Outputs/LastTapPos/Result");
    draggingLabel = GetNode<Label>("Canvas/Outputs/Dragging/Result");

    dayLabel.Text = GameCalendar.I.seasonDaysPassed.ToString();
    seasonLabel.Text = GameCalendar.I.currentSeason.ToString();
    tapPosLabel.Text = "No Tap Yet";
    draggingLabel.Text = "False";
	}

  private void ProgressDay() {
    GameCalendar.I.AdvanceDay();
  }
  private void ProgressSeason() {
    GameCalendar.I.AdvanceSeason();
  }

  private void OnDayPassed(int day) {
    dayLabel.Text = GameCalendar.I.seasonDaysPassed.ToString();
    seasonLabel.Text = GameCalendar.I.currentSeason.ToString();
  }

  private void OnTapped(Vector2 position) {
    tapPosLabel.Text = position.ToString();
  }
  private void OnDragging(Vector2 delta) {
    draggingLabel.Text = (delta != Vector2.Zero).ToString();
  }
}
