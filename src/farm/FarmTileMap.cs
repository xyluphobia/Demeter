using Godot;
using System;

public partial class FarmTileMap : Node2D
{
  public static FarmTileMap I { get; private set; }


  private TileMapLayer groundLayer;
  private TileMapLayer topDirtLayer;
  private TileMapLayer cropLayer;

  private WorkPlotMenu WorkPlotMenu;

	public override void _Ready()
	{
    I = this;

    groundLayer = GetNode<TileMapLayer>("Ground");
    topDirtLayer = GetNode<TileMapLayer>("TopDirt");
    cropLayer = GetNode<TileMapLayer>("Crop");

    WorkPlotMenu = GetNode<WorkPlotMenu>("CanvasLayer/WorkPlotMenu");

    InputManager.I.Tapped += OnTapped;
	}


  private void OnTapped(Vector2 position) {
    if (WorkPlotMenu.Visible) return;
    
    Vector2 worldPos = GetViewport().GetCanvasTransform().AffineInverse() * position;
    Vector2I tappedGridPos = groundLayer.LocalToMap(this.ToLocal(worldPos));
    if (groundLayer.GetCellTileData(tappedGridPos) == null) {
      return;
    }

    PlotObject? plot = FarmManager.I.GetGridPlot(tappedGridPos);
    GD.Print(plot);
    if (plot == null) {
      WorkPlotMenu.Visible = true;
      WorkPlotMenu.activeGridPos = tappedGridPos;
    }
  }


  public void PlantCrop(Vector2I position) {
    cropLayer.SetCell(position, 1, new Vector2I(0, 1));
    GD.Print("planted");
  }

  private void WaterPlot(Vector2I position) {

  }
}
