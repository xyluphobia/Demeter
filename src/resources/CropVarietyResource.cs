using Godot;
using System;

[GlobalClass]
public partial class CropVarietyResource : Resource
{
  [ExportCategory("General")]
  [Export] public string Name { get; set; }

  public enum Tiers { COMMON, HEIRLOOM, HYBRID }
  [Export] public Tiers Tier { get; set; }
  public enum Seasons { SPRING, SUMMER, AUTUMN, WINTER }
  [Export] public Seasons Season { get; set; }
  public enum HarvestTypes { SINGLE, REPEAT }
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
}
