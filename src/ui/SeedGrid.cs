using Godot;
using System;

public partial class SeedGrid : GridContainer
{
  [Export] public PackedScene SeedButtonScene { get; set; }


  public void PopulateSeedGrid(CropVarietyResource[] seeds) {
    foreach (Node child in this.GetChildren()) child.QueueFree();

    foreach (CropVarietyResource crop in seeds) {
      SeedButton button = SeedButtonScene.Instantiate<SeedButton>();
      button.Setup(crop);

      this.AddChild(button);
    }
  }

}
