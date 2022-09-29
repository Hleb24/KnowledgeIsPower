using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy {
  [RequireComponent(typeof(EnemyAnimator))]
  public class EnemyHealth : MonoBehaviour, IHealth {
    public event Action HealthChanged;
    public EnemyAnimator Animator;
    [SerializeField]
    private float _current;
    [SerializeField]
    private float _max;

    public float Current {
      get {
        return _current;
      }
      set {
        _current = value;
      }
    }

    public float Max {
      get {
        return _max;
      }
      set {
        _max = value;
      }
    }

    public void TakeDamage(float damage) {
      Current -= damage;
      Animator.PlayHit();
      HealthChanged?.Invoke();
    }
  }
}