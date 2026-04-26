using Godot;
 
public class LockedPlot : PlotObject 
{

  public LockedPlot(Vector2I GridPosition) : base(GridPosition)
  {
  }

  public override void Cleanup() {}
}
