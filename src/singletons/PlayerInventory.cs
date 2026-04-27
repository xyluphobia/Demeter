using Godot;
using System;

public partial class PlayerInventory : Node
{
  public static PlayerInventory I { get; private set; }

  public override void _Ready() {
    I = this;
  }

  public void AddItem(InventoryItemResource item) {
  }
}
