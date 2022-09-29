using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Enemy {
  public class AgentMoveToPlayer : Follow {
    private const float MINIMUM_DISTANCE = 1;
    public NavMeshAgent Agent;
    private Transform _heroTransform;
    private bool _stopFolLow = false;

    private void Update() {
      if (_stopFolLow) {
        return;
      }
      SetDestinationForAgent();
    }

    public void Construct(Transform heroTransform) {
      _heroTransform = heroTransform;
      Vector3 transformPosition = transform.position;
      transformPosition.y = _heroTransform.position.y;
      transform.position = transformPosition;
    }

    private void SetDestinationForAgent() {
      if (HeroNotReached()) {
        Agent.destination = _heroTransform.position;
      }
    }

    public void StopFollow() {
      _stopFolLow = true;
    }

    private bool HeroNotReached() {
      return Vector3.Distance(Agent.transform.position, _heroTransform.transform.position) >= MINIMUM_DISTANCE;
    }
  }
}