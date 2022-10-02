using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor {
  [CustomEditor(typeof(SpawnMarker))]
  public class SpawnerMarkerEditor : UnityEditor.Editor {
    [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
    public static void RenderCustomGizmo(SpawnMarker spawner, GizmoType gizmo) {
      CircleGizmo(spawner.transform, .5f, Color.red);
    }

    private static void CircleGizmo(Transform spawnerTransform, float radius, Color color) {
      Gizmos.color = color;
      Gizmos.DrawSphere(spawnerTransform.position, radius);
    }
  }
}