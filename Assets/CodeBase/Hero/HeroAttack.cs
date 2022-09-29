using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Hero {
  [RequireComponent(typeof(HeroAnimator)), RequireComponent(typeof(CharacterController))]
  public class HeroAttack : MonoBehaviour, ISaveProgressReader {
    private static int layerMask;
    private const string LAYER_NAME = "Hittable";
    private readonly Collider[] _hits = new Collider [3];
    public HeroAnimator Animator;
    public CharacterController CharacterController;
    private IInputService _input;
    private Stats _stats;

    private void Awake() {
      _input = AllServices.Container.Single<IInputService>();
      layerMask = 1 << LayerMask.NameToLayer(LAYER_NAME);
    }

    private void Update() {
      if (_input.IsAttackButtonUp() && !Animator.IsAttacking) {
        Animator.PlayAttack();
      }
    }

    public void LoadProgress(PlayerProgress progress) {
      Debug.Log("Load Stats");
      _stats = progress.HeroStats;
    }

    private void OnAttack() {
      for (var i = 0; i < Hit(); i++) {
        var health = _hits[i].transform.parent.GetComponent<IHealth>();
        health?.TakeDamage(_stats.Damage);
          PhysicsDebug.DrawDebug(StartPoint(), _stats.DamageRadius, 1.0f);
      }
    }

    private int Hit() {
      return Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, layerMask);
    }

    private Vector3 StartPoint() {
      return new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
    }
  }
}