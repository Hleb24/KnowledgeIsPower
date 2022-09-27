using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Services.Input {
  public interface IInputService : IService {
    bool IsAttackButtonUp();
    public Vector2 Axis { get; }
  }
}