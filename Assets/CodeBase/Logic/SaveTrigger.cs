using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic {
  [RequireComponent(typeof(BoxCollider))]
  public class SaveTrigger : MonoBehaviour {
    public BoxCollider Collider;
    private IStateLoadService _saveLoadService;

    private void Awake() {
      _saveLoadService = AllServices.Container.Single<IStateLoadService>();
    }

    private void OnTriggerEnter(Collider other) {
      _saveLoadService.SaveProgress();
      Debug.Log("Progress save");
      gameObject.SetActive(false);
    }

    private void OnDrawGizmos() {
      Gizmos.color = new Color32(30, 200, 30, 130);
      Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
    }
  }
}