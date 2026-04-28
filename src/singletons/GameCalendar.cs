using Godot;

public partial class GameCalendar : Node
{
  public static GameCalendar I { get; private set; }

  [Signal] public delegate void DayPassedEventHandler(int day);
  [Signal] public delegate void SeasonChangedEventHandler(int season);


  private const int DaysPerSeason = 25;
  public int totalDaysPassed { get; private set; } = 1;
  public int seasonDaysPassed => ((totalDaysPassed - 1) % DaysPerSeason) + 1;
  public Seasons currentSeason { get; private set; } = Seasons.SPRING;

  public override void _Ready()
  {
      I = this;
  }


  public void AdvanceDay() {
    totalDaysPassed ++;

    if (((totalDaysPassed - 1) % DaysPerSeason) + 1 == 1) {
      currentSeason = (Seasons)(((int)currentSeason + 1) % 4);
      EmitSignal(SignalName.SeasonChanged, (int)currentSeason);
    }

    EmitSignal(SignalName.DayPassed, totalDaysPassed);
  }

  public void AdvanceSeason() {
    currentSeason = (Seasons)(((int)currentSeason + 1) % 4);
    EmitSignal(SignalName.SeasonChanged, (int)currentSeason);
  }

}
