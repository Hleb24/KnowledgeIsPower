using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory {
  public interface IGameFactory : IService {
    GameObject CreateHud();
    GameObject CreateHero(GameObject at);
    void CleanUp();
    List<ISaveProgressReader> ProgressReaders { get; }
    List<ISaveProgress> ProgressWriters { get; }
    void Register(ISaveProgressReader progressReader);
    GameObject InstantiateMonster(MonsterId monsterTypeId, Transform parent);
    void Unregister(EnemySpawner enemySpawner);
  }
}