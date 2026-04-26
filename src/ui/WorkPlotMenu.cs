using Godot;
using System;

public partial class WorkPlotMenu : Control
{
  public Vector2I activeGridPos;

  public override void _Ready()
  {
    GetNode<Button>("HBoxContainer/FarmBed").Pressed += OnFarmBedPressed;
    GetNode<Button>("Close").Pressed += ResetMenu;
  }

  private void ResetMenu() {
    activeGridPos = Vector2I.Zero;
    this.Visible = false;
  }

  private void OnFarmBedPressed() {
    FarmManager.I.EmitSignal(FarmManager.SignalName.PlantingAttempting, activeGridPos);
    ResetMenu();
  }

}
