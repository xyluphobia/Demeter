using Godot;
using System.Text;

public partial class Sprint3Debug : Node2D
{
  private Label _inventoryLabel;

	public override void _Ready() {
    _inventoryLabel = GetNode<Label>("CanvasLayer/InventoryDisplay");
    PlayerInventory.I.InventoryChanged += RefreshInventoryLabel;
	}

  private void RefreshInventoryLabel()
  {
    StringBuilder sb = new StringBuilder();
    foreach (InventoryStackResource stack in PlayerInventory.I.GetAllStacks())
        sb.AppendLine($"{stack.Item.Name} x{stack.Quantity}");

    _inventoryLabel.Text = sb.Length > 0 ? sb.ToString().TrimEnd() : "(empty)";
  }
}
