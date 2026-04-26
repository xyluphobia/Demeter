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

    PlotObject? plot = FarmManager.I.GetGridPlot(tappedGridPos);
    if (plot == null) {
      WorkPlotMenu.Visible = true;
      WorkPlotMenu.activeGridPos = tappedGridPos;
    }
  }


  private void OnPlantCrop(Vector2I position, CropVarietyResource crop) {
    Vector2 worldPos = groundLayer.MapToLocal(position);

    Sprite2D sprite = new Sprite2D();
    sprite.Texture = crop.LifeCycleTexture;

    sprite.RegionEnabled = true;
    sprite.RegionRect = new Rect2(0, 0, 16, crop.LifeCycleTexture.GetHeight());
    sprite.Position = worldPos;
    AddChild(sprite);

    FarmManager.I.SetGridPlot(new FarmBedPlot(position) {
      CropSprite = sprite,
      Crop = crop,
    });
  }

  private void WaterPlot(Vector2I position) {

  }
}
