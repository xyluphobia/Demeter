using Godot;
using System;

[GlobalClass]
public partial class RecipeResource : Resource
{
  [ExportCategory("General")]
  [Export] public string Name { get; set; }
  [Export] public int ProcessingTimeDays { get; set; }

  [Export] public (int Amount, InventoryItemResource Ingredient)[] Input { get; set; }
  [Export] public InventoryItemResource Output { get; set; }
}


