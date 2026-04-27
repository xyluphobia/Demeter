using Godot;
using System;

public partial class UIManager : Node
{
  public static UIManager I { get; private set; }

  [Signal] public delegate void OpenSeedSelectionEventHandler();

  public override void _Ready() {
    I = this;
  }

}
