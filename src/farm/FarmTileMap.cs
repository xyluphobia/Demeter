using Godot;
using System;

public partial class FarmTileMap : Node2D
{
  private TileMapLayer groundLayer;
  private TileMapLayer topDirtLayer;
  private TileMapLayer cropLayer;

  private WorkPlotMenu WorkPlotMenu;

	public override void _Ready()
	{
    groundLayer = GetNode<TileMapLayer>("Ground");
    topDirtLayer = GetNode<TileMapLayer>("TopDirt");
    cropLayer = GetNode<TileMapLayer>("Crop");

    WorkPlotMenu = GetNode<WorkPlotMenu>("CanvasLayer/WorkPlotMenu");

    FarmManager.I.RegisterGroundLayer(groundLayer);

    InputManager.I.Tapped += OnTapped;
    FarmManager.I.PlantCrop += OnPlantCrop;
	}


  private void OnTapped(Vector2 position) {
    if (WorkPlotMenu.Visible) return;
    
    Vector2 worldPos = GetViewport().GetCanvasTransform().AffineInverse() * position;
    Vector2I tappedGridPos = groundLayer.LocalToMap(this.ToLocal(worldPos));
    if (groundLayer.GetCellTileData(tappedGridPos) == null) {
      return;
    }

    FarmManager.I.EmitSignal(FarmManager.SignalName.PlotTapped, tappedGridPos);
    WorkPlotMenu.Visible = true;
  }


  private void OnPlantCrop(Vector2I position, CropVarietyResource crop) {
    FarmBedPlot plot = FarmManager.I.GetGridPlot(position) as FarmBedPlot; 
    if (plot.Crop != null)
      return;
    plot.Crop = crop;
  }

  private void WaterPlot(Vector2I position) {

  }
}
