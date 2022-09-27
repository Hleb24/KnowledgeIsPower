namespace SimpleInputNamespace {
  public class AxisInputSwipeGesture : SwipeGestureBase<string, float> {
    public SimpleInput.AxisInput axis = new();
    public float value = 1f;

    public override int Priority {
      get {
        return 1;
      }
    }

    protected override BaseInput<string, float> Input {
      get {
        return axis;
      }
    }

    protected override float Value {
      get {
        return value;
      }
    }
  }
}