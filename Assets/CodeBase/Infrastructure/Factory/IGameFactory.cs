using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory {
  public interface IGameFactory : IService {
    Task<GameObject> CreateHud();
    Task<GameObject> CreateHero(LevelStaticData at);
    void CleanUp();
    List<ISaveProgressReader> ProgressReaders { get; }
    List<ISaveProgress> ProgressWriters { get; }
    Task<GameObject> InstantiateMonster(MonsterId monsterTypeId, Transform parent);
    void Unregister(SpawnPoint spawnPoint);
    Task<LootPiece> CreateLoot();
    Task CreateSpawner(Vector3 at, string spawnerId, MonsterId monsterId);
    Task WarmUp();
  }
}