using Godot;
using System;

public partial class InputManager : Node
{
  public static InputManager I { get; private set; }

  [Signal] public delegate void TappedEventHandler(Vector2 position);
  [Signal] public delegate void DraggingEventHandler(bool dragging);
  [Signal] public delegate void PinchChangedEventHandler(float delta);

  private Vector2 _touch0Position;
  private Vector2 _touch1Position;
  private Vector2 _touch0StartPosition;
  private float _lastPinchDistance;
  private bool _isDragging;

  private const float TapThreshold = 10f;

  public override void _Ready()
  {
      I = this;
  }


  public override void _Input(InputEvent @event) {
    switch(@event)
    {
      case InputEventScreenTouch touch:
        if (touch.Pressed) {
          if (touch.Index == 0) {
            _touch0Position = touch.Position;
            _touch0StartPosition = touch.Position;
            _isDragging = false;
            EmitSignal(SignalName.Dragging, _isDragging);
          }
          else if (touch.Index == 1) {
            _touch1Position = touch.Position;
            _lastPinchDistance = 0;
          }
        }
        else { // Release
          if (touch.Index == 0) {
            float distanceMoved = _touch0StartPosition.DistanceTo(touch.Position);
            if (distanceMoved < TapThreshold && !_isDragging) {
              EmitSignal(SignalName.Tapped, touch.Position);
            }
          }
          _isDragging = false;
          EmitSignal(SignalName.Dragging, _isDragging);
        }
        break;

      case InputEventScreenDrag drag:
        if (drag.Index == 0) {
            _touch0Position = drag.Position;
            if (_touch0StartPosition.DistanceTo(drag.Position) > TapThreshold) {
              _isDragging = true;
              EmitSignal(SignalName.Dragging, _isDragging);
            }
        }
        else if (drag.Index == 1) {
            _touch1Position = drag.Position;
        }

        // Check for pinch after updating either finger
        if (_lastPinchDistance > 0) {
            float currentDistance = _touch0Position.DistanceTo(_touch1Position);
            float delta = currentDistance - _lastPinchDistance;
            EmitSignal(SignalName.PinchChanged, delta);
            _lastPinchDistance = currentDistance;
        }
        else if (_touch1Position != Vector2.Zero) {
            // Start tracking pinch distance once we have both positions
            _lastPinchDistance = _touch0Position.DistanceTo(_touch1Position);
        }
        break;
    }
  }

}
