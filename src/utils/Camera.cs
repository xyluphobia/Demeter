using Godot;
using System;

public partial class Camera : Camera2D
{
  private const float ZoomSensitivity = 0.001f;
  private const float ZoomDuration = 0.15f;
  private static readonly Vector2 ZoomMin = Vector2.One * 1.5f;
  private static readonly Vector2 ZoomMax = Vector2.One * 3.0f;

  private Vector2 _targetZoom;
  private Tween _zoomTween;


	public override void _Ready()
	{
    _targetZoom = this.Zoom;

    InputManager.I.PinchChanged += OnPinch;
    InputManager.I.Dragging += OnDrag;
	}

  private void OnPinch(float delta) {
    _targetZoom = (_targetZoom + Vector2.One * delta * ZoomSensitivity).Clamp(ZoomMin, ZoomMax);

    _zoomTween?.Kill();
    _zoomTween = CreateTween();
    _zoomTween.TweenProperty(this, "zoom", _targetZoom, ZoomDuration)
      .SetTrans(Tween.TransitionType.Cubic)
      .SetEase(Tween.EaseType.Out);
  }

  private void OnDrag(Vector2 delta) {
    if (delta == Vector2.Zero) return;

    this.Position -= delta / this.Zoom;
  }
}
