using UnityEngine;

namespace CodeBase.Services.Input {
  public abstract class InputService : IInputService {
    protected static Vector2 GetSimpleInputAxis() {
      return new Vector2(SimpleInput.GetAxis(HORIZONTAL), SimpleInput.GetAxis(VERTICAL));
    }

    protected const string HORIZONTAL = "Horizontal";
    protected const string VERTICAL = "Vertical";
    private const string BUTTON = "Fire";

    public bool IsAttackButtonUp() {
      return SimpleInput.GetButtonUp(BUTTON);
    }

    public abstract Vector2 Axis { get; }
  }
}