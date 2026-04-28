using Godot;

[GlobalClass]
public abstract partial class CollectibleItemResource : Resource
{
  [Export] public string Name { get; set; }
  [Export] public int BaseSellPrice { get; set; }
  [Export] public int ShelfLifeDays { get; set; }
}

