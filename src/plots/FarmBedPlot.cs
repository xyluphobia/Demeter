using Godot;
 
public class FarmBedPlot : PlotObject 
{
  public CropVarietyResource Crop { get; set; }
  public Sprite2D CropSprite  { get; set; }

  public FarmBedPlot(Vector2I GridPosition) : base(GridPosition)
  {
  }
}
