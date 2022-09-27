using CodeBase.Data;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Hero {
  public class HeroMove : MonoBehaviour, ISaveProgress {
    private static string CurrentLevel() {
      return SceneManager.GetActiveScene().name;
    }

    public CharacterController CharacterController;
    public float MovementSpeed;
    private IInputService _inputService;
    private Camera _camera;
    private HeroAnimator _heroAnimator;

    private void Awake() {
      _inputService = AllServices.Container.Single<IInputService>();
      CharacterController = GetComponent<CharacterController>();
      _heroAnimator = GetComponent<HeroAnimator>();
      _camera = Camera.main;
    }

    private void Update() {
      Vector3 movementVector = Vector3.zero;
      if (!_heroAnimator.IsAttacking && _inputService.Axis.sqrMagnitude > Constants.Epsilon) {
        movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        movementVector.y = 0;
        movementVector.Normalize();
        transform.forward = movementVector;
      }

      movementVector += Physics.gravity;
      CharacterController.Move(movementVector * (MovementSpeed * Time.deltaTime));
    }

    public void LoadProgress(PlayerProgress progress) {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void UpdateProgress(PlayerProgress progress) {
      if (CurrentLevel() == progress.WorldData.PositionOnLevel.Level) {
        Vector3Data savePosition = progress.WorldData.PositionOnLevel.Position;
        if (savePosition != null) {
          Warp(savePosition);
        }
      }
    }

    private void Warp(Vector3Data to) {
      CharacterController.enabled = false;
      transform.position = to.AsUnityVector().AddY(CharacterController.height);
      CharacterController.enabled = true;
    }
  }
}