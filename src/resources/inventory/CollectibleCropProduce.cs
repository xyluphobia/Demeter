using Godot;
using System;

[GlobalClass]
public partial class CollectibleCropProduce : CollectibleItemResource
{
  /* SHELF LIFE STORED IN ABSTRACT */

  public enum Categories { FRESH, PROCESSED, SEED }
  [Export] public Categories Category { get; set; }
}

