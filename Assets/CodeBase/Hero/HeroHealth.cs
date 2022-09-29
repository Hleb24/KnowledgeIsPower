using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero {
  [RequireComponent(typeof(HeroAnimator))]
  public class HeroHealth : MonoBehaviour, ISaveProgress, IHealth {
    public event Action HealthChanged;
    public HeroAnimator Animator;
    private State _state;

    public void TakeDamage(float damage) {
      Debug.Log("Take Damage");
      if (Current <= 0) {
        return;
      }

      Current -= damage;
      Debug.Log($" Damage {damage} current {Current}");

      Animator.PlayHit();
    }

    public void LoadProgress(PlayerProgress progress) {
      _state = progress.HeroState;
      HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress) {
      progress.HeroState.CurrentHP = Current;
      progress.HeroState.MaxHP = Max;
    }

    public float Max {
      get {
        return _state.MaxHP;
      }
      set {
        _state.MaxHP = value;
      }
    }

    public float Current {
      get {
        return _state.CurrentHP;
      }
      set {
        Debug.Log($"Change Current {value}");
        if (_state.CurrentHP != value) {
          Debug.Log("Change Current return");
          _state.CurrentHP = value;
          HealthChanged?.Invoke();
        }
      }
    }
  }
}