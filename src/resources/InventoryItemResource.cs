using Godot;
using System;

#nullable enable

[GlobalClass]
public partial class InventoryItemResource : Resource
{
  [ExportCategory("General")]
  [Export] public required string Name { get; set; }

  public enum Categories { FRESH, PROCESSED, SEED }
  [Export] public Categories Category { get; set; }

  [Export] public int BaseSellPrice { get; set; }
  [Export] public int ShelfLifeDays { get; set; }
  [Export] public CropVarietyResource? CropVariety { get; set; }
}

