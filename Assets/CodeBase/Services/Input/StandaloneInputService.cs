using UnityEngine;

namespace CodeBase.Services.Input {
  public class StandaloneInputService : InputService {
    private static Vector2 GetUnityAxis() {
      return new Vector2(UnityEngine.Input.GetAxis(HORIZONTAL), UnityEngine.Input.GetAxis(VERTICAL));
    }

    public override Vector2 Axis {
      get {
        Vector2 axis = GetSimpleInputAxis();
        if (axis == Vector2.zero) {
          axis = GetUnityAxis();
        }

        return axis;
      }
    }
  }
}