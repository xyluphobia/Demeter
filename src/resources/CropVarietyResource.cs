using Godot;
using System;

[GlobalClass]
public partial class CropVarietyResource : Resource
{
  [ExportCategory("General")]
  [Export] public string Name { get; set; }
  [Export] public Plants Plant { get; set; }
  [Export] public LifeCycles LifeCycle { get; set; }

  [Export] public Tiers Tier { get; set; }
  [Export] public Seasons Season { get; set; }
  [Export] public HarvestTypes HarvestType { get; set; }

  [Export] public int MaturityDays { get; set; }
  [Export] public int ReharvestCycleDays { get; set; }

  [Export] public float BaseSellPrice { get; set; }

  [ExportCategory("Stats")]
  [Export(PropertyHint.Range, "1,100")] public int Flavour { get; set; }
  [Export(PropertyHint.Range, "1,100")] public int Yield { get; set; }
  [Export(PropertyHint.Range, "1,100")] public int ShelfLife { get; set; }
  [Export(PropertyHint.Range, "1,100")] public int Resilience { get; set; }
  [Export(PropertyHint.Range, "1,100")] public int GrowthSpeed { get; set; }

  [ExportCategory("Visuals")]
  [Export] public Texture2D SeedPacketTexture { get; set; }
  [Export] public Texture2D LifeCycleTexture { get; set; }
  [Export] public int MaxTextureHeight { get; set; }
}
