using Godot;

[GlobalClass]
public partial class InventoryStackResource : Resource
{
  [Export] public CollectibleItemResource Item { get; set; }
  [Export] public int Quantity { get; set; }
  [Export] public int CollectedOnDay { get; set; }

  public int DaysUntilExpiry => Item is null ? 0 : 
    (CollectedOnDay + Item.ShelfLifeDays) - GameCalendar.I.totalDaysPassed;
}


