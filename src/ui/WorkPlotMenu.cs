using Godot;
using System;

public partial class WorkPlotMenu : Control
{
  private Vector2I _activeGridPos;

  private Control farmBedButtons;
  private Control emptyPlotButtons;

  private Control confirmPullDialogue;

  public override void _Ready()
  {
    farmBedButtons = GetNode<HBoxContainer>("FarmBedOptions");
    confirmPullDialogue = GetNode<Control>("ConfirmDialogue");
    emptyPlotButtons = GetNode<Control>("EmptyBedOptions");

    GetNode<Button>("Close").Pressed += ResetMenu;

    farmBedButtons.GetNode<Button>("Plant").Pressed += OnPlant;
    farmBedButtons.GetNode<Button>("Water").Pressed += OnWater;
    farmBedButtons.GetNode<Button>("Harvest").Pressed += OnHarvest;
    farmBedButtons.GetNode<Button>("Pull").Pressed += OnPull;

    confirmPullDialogue.GetNode<TextureButton>("Buttons/CancelButton").Pressed += OnPullCancel;
    confirmPullDialogue.GetNode<TextureButton>("Buttons/ConfirmButton").Pressed += OnPullConfirm;

    emptyPlotButtons.GetNode<Button>("Till").Pressed += OnTill;

    FarmManager.I.PlotTapped += OnPlotTapped;
  }

  private void ResetMenu() {
    _activeGridPos = Vector2I.Zero;

    confirmPullDialogue.Visible = false;
    farmBedButtons.Visible = false;
    emptyPlotButtons.Visible = false;
    this.Visible = false;
  }

  private void OnPlotTapped(Vector2I plotPosition) {
    _activeGridPos = plotPosition;

    FarmManager.I.ActiveGridPos = plotPosition;
    PlotObject plot = FarmManager.I.GetGridPlot(plotPosition);

    switch (plot) {
      case FarmBedPlot farmBedPlot:
        // Plant/Water/Harvest/Pull
        farmBedButtons.Visible = true;
        break;
      case EmptyPlot emptyPlot:
        // Make into FarmBedPlot
        emptyPlotButtons.Visible = true;
        break;
      case LockedPlot lockedPlot:
        // Inaccesable
        GD.Print("Locked");
        break;
    }
  }

  private void OnPlant() {
    UIManager.I.EmitSignal(UIManager.SignalName.OpenSeedSelection);
    ResetMenu();
  }
  private void OnWater() {
  }
  private void OnHarvest() {
    (FarmManager.I.GetGridPlot(_activeGridPos) as FarmBedPlot)?.Harvest();
    ResetMenu();
  }
  private void OnPull() {
    if ((FarmManager.I.GetGridPlot(_activeGridPos) as FarmBedPlot)?.Crop != null)
      confirmPullDialogue.Visible = true;
  }
  
  private void OnPullConfirm() {
    (FarmManager.I.GetGridPlot(_activeGridPos) as FarmBedPlot)?.ClearCrop();
    ResetMenu();
  }
  private void OnPullCancel() {
    ResetMenu();
  }

  private void OnTill() {
    FarmManager.I.SetGridPlot(new FarmBedPlot(_activeGridPos));
    ResetMenu();
  }

}
