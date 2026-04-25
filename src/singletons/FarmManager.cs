using Godot;
using System;
using System.Collections.Generic;

public partial class FarmManager : Node
{
  public static FarmManager I { get; private set; }


  private Dictionary<Vector2I, PlotObject> grid = new();
     

  public override void _Ready()
  {
      I = this;
  }

  public PlotObject? GetGridPlot(Vector2I position) {
    if (grid.ContainsKey(position)) {
      return grid[position];
    }
    return null;
  }

  public void SetGridPlot(PlotObject plot) {
    grid[plot.GridPosition] = plot;
  }
}
