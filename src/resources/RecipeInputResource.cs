using Godot;
using System;

[GlobalClass]
public partial class RecipeInputResource : Resource
{
  [Export] public int Amount { get; set; }
  [Export] public InventoryItemResource Ingredient { get; set; }
}



