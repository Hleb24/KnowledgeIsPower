using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory {
  public interface IGameFactory : IService {
    GameObject CreateHud();
    GameObject CreateHero(GameObject at);
    void CleanUp();
    List<ISaveProgressReader> ProgressReaders { get; }
    List<ISaveProgress> ProgressWriters { get; }
    GameObject InstantiateMonster(MonsterId monsterTypeId, Transform parent);
    void Unregister(SpawnPoint spawnPoint);
    LootPiece CreateLoot();
    void CreateSpawner(Vector3 at, string spawnerId, MonsterId monsterId);
  }
}