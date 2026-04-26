using Godot;
using System;

public partial class SeedButton : Button
{
  [Signal] public delegate void SeedPressedEventHandler(CropVarietyResource crop);

  private CropVarietyResource _crop;

  public void Setup(CropVarietyResource crop)
  {
    if (crop == null) {
      this.Disabled = true;
      return;
    }
  
    if (crop.SeedPacketTexture != null) {
      GetNode<TextureRect>("SeedIcon").Texture = crop.SeedPacketTexture;
      this.Icon = null;
    }
    _crop = crop;
    this.Pressed += OnSeedButtonPressed;
  }

  private void OnSeedButtonPressed() {
    EmitSignal(SignalName.SeedPressed, _crop);
  }
}
