using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic.EnemySpawners {
  public class SpawnPoint : MonoBehaviour, ISaveProgress {
    public MonsterId MonsterTypeId;
    public bool Slain;
    public string ID { get; set; }
    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;

    public void Construct(IGameFactory factory) {
      _factory = factory;
    }
    private void OnDestroy() {
      _factory.Unregister(this);
    }

    public void LoadProgress(PlayerProgress progress) {
      if (progress.KillData.ClearedSpawners.Contains(ID)) {
        Slain = true;
      } else {
        Spawn();
      }
    }

    public void UpdateProgress(PlayerProgress progress) {
      if (Slain) {
        progress.KillData.ClearedSpawners.Add(ID);
      }
    }

    private void Spawn() {
      GameObject monster = _factory.InstantiateMonster(MonsterTypeId, transform);
      _enemyDeath = monster.GetComponent<EnemyDeath>();
      _enemyDeath.Happened += Slay;
    }

    private void Slay() {
      if (_enemyDeath != null) {
        _enemyDeath.Happened -= Slay;
      }

      Slain = true;
    }
  }
}