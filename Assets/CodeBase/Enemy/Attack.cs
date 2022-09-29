using System.Linq;
using CodeBase.Logic;
using JetBrains.Annotations;
using UnityEngine;

namespace CodeBase.Enemy {
  [RequireComponent(typeof(EnemyAnimator))]
  public class Attack : MonoBehaviour {
    private readonly Collider[] _hits = new Collider[1];
    public EnemyAnimator Animator;
    public float AttackCooldown = 3f;
    public float Cleavage = 5.0f;
    public float EffectiveDistance = .5f;
    public float Damage = 10f;
    private Transform _heroTransform;
    private float _attackCooldown;
    private bool _isAttacking;
    private int _layerMask;
    private bool _attackIsActive;

    private void Awake() {
      _layerMask = 1 << LayerMask.NameToLayer("Player");
    }

    private void Update() {
      UpdateCooldown();
      if (CanAttack()) {
        Debug.Log("CanAttack");
        StartAttack();
      }
    }

    public void Construct(Transform heroTransform) {
      _heroTransform = heroTransform;
    }

    public void EnabledAttack() {
      _attackIsActive = true;
    }

    public void DisableAttack() {
      _attackIsActive = false;
    }

    private void UpdateCooldown() {
      if (!CooldownIsUp()) {
        _attackCooldown -= Time.deltaTime;
      }
    }

    [UsedImplicitly]
    private void OnAttackEnded() {
      Debug.Log("OnAttackEnded");
      _attackCooldown = AttackCooldown;
      _isAttacking = false;
    }

    [UsedImplicitly]
    private void OnAttack() {
      Debug.Log("OnAttack");
      if (Hit(out Collider hit)) {
        PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1.0f);
        hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
      }
    }

    private bool Hit(out Collider hit) {
      int hitCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

      hit = _hits.FirstOrDefault();

      return hitCount > 0;
    }

    private Vector3 StartPoint() {
      return new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;
    }

    private void StartAttack() {
      transform.LookAt(_heroTransform);
      Animator.PlayAttack();
      _isAttacking = true;
    }

    private bool CooldownIsUp() {
      return _attackCooldown <= 0f;
    }

    private bool CanAttack() {
      return _attackIsActive && !_isAttacking && CooldownIsUp();
    }
  }
}