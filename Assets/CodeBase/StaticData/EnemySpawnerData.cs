using System;
using UnityEngine;

namespace CodeBase.StaticData {
  [Serializable]
  public class EnemySpawnerData {
    public string Id;
    public MonsterId MonsterId;
    public Vector3 Position;
    public EnemySpawnerData(string id, MonsterId monsterId, Vector3 position) {
      Id = id;
      MonsterId = monsterId;
      Position = position;
    }
  }
}