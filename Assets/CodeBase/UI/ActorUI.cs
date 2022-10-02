using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI {
  public class ActorUI : MonoBehaviour {
    public HpBar HpBar;
    private IHealth _health;

    private void Start() {
      PrepareHealth();
    }

    private void OnDestroy() {
      _health.HealthChanged -= UpdateHpBar;
    }

    public void Construct(IHealth heroHealth) {
      _health = heroHealth;
      _health.HealthChanged += UpdateHpBar;
    }

    private void PrepareHealth() {
      if (_health == null) {
        _health = GetComponentInChildren<IHealth>();
        _health.HealthChanged += UpdateHpBar;
      }
    }

    private void UpdateHpBar() {
      HpBar.SetValue(_health.Current, _health.Max);
    }
  }
}