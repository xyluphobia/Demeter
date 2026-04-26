using Godot;
using System;

public partial class SeedGrid : GridContainer
{
  [Export] public PackedScene SeedButtonScene { get; set; }
  [Signal] public delegate void SeedPressedEventHandler(CropVarietyResource crop);

  public void PopulateSeedGrid(CropVarietyResource[] seeds) {
    foreach (Node child in this.GetChildren()) child.QueueFree();

    foreach (CropVarietyResource crop in seeds) {
      SeedButton button = SeedButtonScene.Instantiate<SeedButton>();
      button.Setup(crop);
      button.SeedPressed += (crop) => EmitSignal(SignalName.SeedPressed, crop);

      this.AddChild(button);
    }
  }

}
