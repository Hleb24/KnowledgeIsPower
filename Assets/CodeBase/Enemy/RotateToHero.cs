using UnityEngine;

namespace CodeBase.Enemy {
  public class RotateToHero : Follow {
    public float Speed;
    private Transform _heroTransform;
    private Vector3 _positionToLook;

    private void Update() {
      if (Initialized()) {
        RotateTowardsHero();
      }
    }

    public void Construct(Transform heroTransform) {
      _heroTransform = heroTransform;
    }

    private void RotateTowardsHero() {
      UpdatePositionToLookAt();
      transform.rotation = SmoothedRotation(transform.rotation, _positionToLook);
    }

    private void UpdatePositionToLookAt() {
      Vector3 diff = _heroTransform.position = transform.position;
      _positionToLook = new Vector3(diff.x, transform.position.y, diff.z);
    }

    private Quaternion SmoothedRotation(Quaternion transformRotation, Vector3 positionToLook) {
      return Quaternion.Lerp(transformRotation, TargetRotation(positionToLook), SpeedFactor());
    }

    private Quaternion TargetRotation(Vector3 positionToLook) {
      return Quaternion.LookRotation(positionToLook);
    }

    private float SpeedFactor() {
      return Speed * Time.deltaTime;
    }

    private bool Initialized() {
      return _heroTransform != null;
    }
  }
}