using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy {
  [RequireComponent(typeof(EnemyHealth)), RequireComponent(typeof(EnemyAnimator))]
  public class EnemyDeath : MonoBehaviour {
    public event Action Happened;
    public EnemyHealth Health;
    public EnemyAnimator Animator;
    public AgentMoveToPlayer Follow;

    public GameObject DeathFx;
    private bool _isDeath;

    private void Start() {
      Health.HealthChanged += OnHealthChanged;
    }

    private void OnDestroy() {
      Health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged() {
      if (!_isDeath && Health.Current <= 0) {
        Die();
      }
    }

    private void Die() {
      Health.HealthChanged -= OnHealthChanged;
      Follow.StopFollow();
      _isDeath = true;
      Animator.PlayDeath();
      SpawnDeathFx();
      StartCoroutine(DestroyTime());
      Happened?.Invoke();
    }

    private GameObject SpawnDeathFx() {
      return Instantiate(DeathFx, transform.position, Quaternion.identity);
    }

    private IEnumerator DestroyTime() {
      yield return new WaitForSeconds(3.0f);
      Destroy(gameObject);
    }
  }
}