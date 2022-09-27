namespace SimpleInputNamespace {
  public class MouseButtonInputSwipeGesture : SwipeGestureBase<int, bool> {
    public SimpleInput.MouseButtonInput mouseButton = new();

    public override int Priority {
      get {
        return 1;
      }
    }

    protected override BaseInput<int, bool> Input {
      get {
        return mouseButton;
      }
    }

    protected override bool Value {
      get {
        return true;
      }
    }
  }
}