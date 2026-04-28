using Godot;

[GlobalClass]
public partial class RecipeResource : Resource
{
  [ExportCategory("General")]
  [Export] public string Name { get; set; }
  [Export] public int ProcessingTimeDays { get; set; }

  [Export] public RecipeInputResource[] Input { get; set; }
  [Export] public CollectibleItemResource Output { get; set; }
}


