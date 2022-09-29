using UnityEngine;

namespace CodeBase.Hero {
  [RequireComponent(typeof(HeroHealth))]
  public class HeroDeath : MonoBehaviour {
    public HeroHealth Health;
    public HeroMove Move;
    public HeroAnimator Animator;
    public HeroAttack HeroAttack;
    public GameObject DeathFx;
    private bool _isDead;

    private void Start() {
      Health.HealthChanged += OnHealthChanged;
    }

    private void OnDestroy() {
      Health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged() {
      if (!_isDead && Health.Current <= 0) {
        Die();
      }
    }

    private void Die() {
      _isDead = true;
      Move.enabled = false;
      HeroAttack.enabled = false;
      Animator.PlayDeath();

      Instantiate(DeathFx, transform.position, Quaternion.identity);
    }
  }
}