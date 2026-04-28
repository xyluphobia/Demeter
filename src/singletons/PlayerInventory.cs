using Godot;
using System;
using System.Linq;
using System.Collections.Generic;

public partial class PlayerInventory : Node
{
  public static PlayerInventory I { get; private set; }

  [Signal] public delegate void InventoryChangedEventHandler();
  
  // Used only for saving and loading, this datastructue is better for that within godot
  [Export] public Godot.Collections.Array<InventoryStackResource> Items { get; private set; } = new();

  // Used for runtime lookup/inventory management
  private Dictionary<CollectibleItemResource, List<InventoryStackResource>> _inventory = new ();

  public override void _Ready() {
    I = this;
    RebuildDictionary();
    GameCalendar.I.DayPassed += _ => ProcessExpiry();
  }

  // Each item can have multiple stacks with different info, mostly
  // useful so that all carrots don't expire on the same day etc
  public void AddItem(CollectibleItemResource item, int quantity = 1) {
    if (item == null) {
      GD.PushWarning("PlayerInventory.AddItem called with null item — check Produce is assigned on the CropVarietyResource.");
      return;
    }

    InventoryStackResource stack = new InventoryStackResource {
      Item = item,
      Quantity = quantity,
      CollectedOnDay = GameCalendar.I.totalDaysPassed
    };

    if (_inventory.ContainsKey(item))
      _inventory[item].Add(stack);
    else
      _inventory[item] = new List<InventoryStackResource> { stack };

    EmitSignal(SignalName.InventoryChanged);
  }

  public void RemoveItem(CollectibleItemResource item, int quantity = 1) {
    if (!_inventory.TryGetValue(item, out List<InventoryStackResource> stacks)) return;

    // FIFO
    int remainingToRemove = quantity;
    while (remainingToRemove > 0 && stacks.Count > 0) {
      InventoryStackResource oldest = stacks[0];
      if (oldest.Quantity <= remainingToRemove) {
        remainingToRemove -= oldest.Quantity;
        stacks.RemoveAt(0);
      }
      else {
        oldest.Quantity -= remainingToRemove;
        remainingToRemove = 0;
      }
    }

    if (stacks.Count == 0)
      _inventory.Remove(item);

    EmitSignal(SignalName.InventoryChanged);
  }

  public int GetQuantity(CollectibleItemResource item) {
    if (!_inventory.TryGetValue(item, out List<InventoryStackResource> stacks)) return 0;
    return stacks.Sum(s => s.Quantity);
  }

  public bool HasItem(CollectibleItemResource item, int quantity = 1) => GetQuantity(item) >= quantity;

  // Called to expire old stacks
  public void ProcessExpiry() {
    int today = GameCalendar.I.totalDaysPassed;
    foreach (KeyValuePair<CollectibleItemResource, List<InventoryStackResource>> entry in _inventory) {
      entry.Value.RemoveAll(s =>
          s.CollectedOnDay + entry.Key.ShelfLifeDays < today);
    }

    List<CollectibleItemResource> empty = _inventory.Keys.Where(k => _inventory[k].Count == 0)
                               .ToList();
    foreach (CollectibleItemResource key in empty)   
      _inventory.Remove(key);
  }

  public IEnumerable<InventoryStackResource> GetAllStacks()
  {
    foreach (List<InventoryStackResource> stacks in _inventory.Values)
      foreach (InventoryStackResource stack in stacks)
        yield return stack;
  }

  public void Save() {
    Items.Clear();
    foreach (List<InventoryStackResource> stacks in _inventory.Values)
      foreach (InventoryStackResource stack in stacks)
        Items.Add(stack);

    InventorySaveData saveData = new InventorySaveData { Items = Items };
    ResourceSaver.Save(saveData, "user://inventory.tres");
  }
  public void Load() {
    if (!FileAccess.FileExists("user://inventory.tres")) return;
    InventorySaveData saved = ResourceLoader.Load<InventorySaveData>("user://inventory.tres");
    Items = saved.Items;
    RebuildDictionary();
  }
  private void RebuildDictionary() {
    _inventory.Clear();
    foreach(InventoryStackResource stack in Items) {
      if (!_inventory.ContainsKey(stack.Item))
        _inventory[stack.Item] = new List<InventoryStackResource>();
      _inventory[stack.Item].Add(stack);
    }
  }
}

[GlobalClass]
public partial class InventorySaveData : Resource
{
  [Export] public Godot.Collections.Array<InventoryStackResource> Items { get; set; } = new();
}
