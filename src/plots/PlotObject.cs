using Godot;
using System;

public abstract class PlotObject
{
  public Vector2I GridPosition { get; set; }

  protected PlotObject(Vector2I gridPosition)
  {
    GridPosition = gridPosition;
  }
}
