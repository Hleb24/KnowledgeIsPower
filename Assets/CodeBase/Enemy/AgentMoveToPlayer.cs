using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy {
  public class AgentMoveToPlayer : Follow {
    private const float MINIMUM_DISTANCE = 1;
    public NavMeshAgent Agent;
    private Transform _heroTransform;
    private IGameFactory _gameFactory;

    private void Start() {
      _gameFactory = AllServices.Container.Single<IGameFactory>();
      if (_gameFactory.HeroGameObject != null) {
        InitializeHeroTransform();
      } else {
        _gameFactory.HeroCreated += OnCreateHero;
      }
    }

    private void Update() {
      if (Initialized() && HeroNotReached()) {
        Agent.destination = _heroTransform.position;
      }
    }

    private void InitializeHeroTransform() {
      _heroTransform = _gameFactory.HeroGameObject.transform;
    }

    private bool Initialized() {
      return _heroTransform != null;
    }

    private void OnCreateHero() {
      InitializeHeroTransform();
      _gameFactory.HeroCreated -= OnCreateHero;
    }

    private bool HeroNotReached() {
      return Vector3.Distance(Agent.transform.position, _heroTransform.transform.position) >= MINIMUM_DISTANCE;
    }
  }
}