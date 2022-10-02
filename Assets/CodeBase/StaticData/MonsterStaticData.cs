using UnityEngine;

namespace CodeBase.StaticData {
  [CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster", order = 0)]
  public class MonsterStaticData : ScriptableObject {
    public MonsterId MonsterId;
    [Range(1, 100)]
    public int HP;
    [Range(1f, 30f)]
    public float Damage;
    public int MinLoot;
    public int MaxLoot;
    [Range(0.5f, 1)]
    public float EffectiveDistance = .666f;
    [Range(0.5f, 1)]
    public float Cleavage;
    public float MoveSpeed = 3f;

    public GameObject Prefab;
  }
}