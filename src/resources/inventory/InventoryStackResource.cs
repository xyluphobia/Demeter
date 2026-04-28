using Godot;
using System;

#nullable enable

[GlobalClass]
public partial class InventoryStackResource : Resource
{
  [Export] public CollectibleItemResource Item { get; set; }
  [Export] public int Quantity { get; set; }
  [Export] public int CollectedOnDay { get; set; }
}


