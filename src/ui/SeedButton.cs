using Godot;
using System;

public partial class SeedButton : Button
{
  private CropVarietyResource _crop;

  public void Setup(CropVarietyResource crop)
  {
    if (crop == null) {
      this.Disabled = true;
      return;
    }
  
    // GetNode<TextureRect>("TextureRect").Texture = /* crop texture */;
    _crop = crop;
    this.Pressed += OnSeedButtonPressed;
  }

  private void OnSeedButtonPressed() {
    
  }
}
