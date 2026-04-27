using Godot;
using System;
using System.Collections.Generic;

public partial class FarmManager : Node
{
  public static FarmManager I { get; private set; }

  [Signal] public delegate void PlotTappedEventHandler(Vector2I plotPosition);
  [Signal] public delegate void PlantCropEventHandler(Vector2I position, CropVarietyResource crop);

  public Node2D CropSpriteContainer { get; private set; }
  public TileMapLayer GroundLayer { get; private set; }

  private Dictionary<Vector2I, PlotObject> grid = new();
  public Vector2I ActiveGridPos;

  public override void _Ready()
  {
      I = this;

      CropSpriteContainer = new Node2D { Name = "CropSpriteContainer" };
      GetTree().Root.CallDeferred("add_child", CropSpriteContainer);
  }
  public void RegisterGroundLayer(TileMapLayer groundLayer) {
    GroundLayer = groundLayer;
  }

  public PlotObject GetGridPlot(Vector2I position) {
    if (grid.ContainsKey(position)) {
      return grid[position];
    }
    return SetGridPlot(new EmptyPlot(position));
  }

  public PlotObject SetGridPlot(PlotObject plot) {
    if (grid.ContainsKey(plot.GridPosition))
      grid[plot.GridPosition].Cleanup();

    grid[plot.GridPosition] = plot;
    return plot;
  }

  public Vector2 GridToWorld(Vector2I gridPosition) {
    return GroundLayer.MapToLocal(gridPosition);
  }

  public void HarvestPlot(FarmBedPlot plot) {
    PlayerInventory.I.AddItem(plot.Crop.Produce);
    plot.Harvest();

    if (plot.Crop.HarvestType == HarvestTypes.SINGLE)
      EmptyPlot(plot);
  }

  public void EmptyPlot(PlotObject plot) {
    grid[plot.GridPosition] = new EmptyPlot(plot.GridPosition);
    plot.Cleanup();
  }
}
