using UnityEngine;

namespace SimpleInputNamespace {
  public interface IBaseInput {
    void StartTracking();
    void StopTracking();
    void ResetValue();
  }

  public abstract class BaseInput<K, V> : IBaseInput {
    public V value;
    private bool isTracking;

    public BaseInput() { }

    public BaseInput(K key) {
      m_key = key;
    }

    public void StartTracking() {
      if (!isTracking) {
        if (IsKeyValid()) {
          RegisterInput();
        }

        isTracking = true;
      }
    }

    public void StopTracking() {
      if (isTracking) {
        if (IsKeyValid()) {
          UnregisterInput();
        }

        isTracking = false;
      }
    }

    public void ResetValue() {
      value = default;
    }

    public virtual bool IsKeyValid() {
      return true;
    }

    protected abstract void RegisterInput();
    protected abstract void UnregisterInput();
    protected abstract bool KeysEqual(K key1, K key2);
#pragma warning disable 0649
    [SerializeField]
    private K m_key;
    public K Key {
      get {
        return m_key;
      }
      set {
        if (!KeysEqual(m_key, value)) {
          if (isTracking && IsKeyValid()) {
            UnregisterInput();
          }

          m_key = value;

          if (isTracking && IsKeyValid()) {
            RegisterInput();
          }
        }
      }
    }
#pragma warning restore 0649
  }
}