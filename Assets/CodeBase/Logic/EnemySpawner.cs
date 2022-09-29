﻿using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using UnityEngine;

namespace CodeBase.Logic {
  public class EnemySpawner : MonoBehaviour, ISaveProgress {
    public MonsterId MonsterTypeId;
    public bool Slain;
    private string _id;
    private IGameFactory _factory;
    private EnemyDeath _enemyDeath;

    private void Awake() {
      _id = GetComponent<UniqueId>().Id;
      _factory = AllServices.Container.Single<IGameFactory>();
    }

    private void OnDestroy() {
      _factory.Unregister(this);
    }

    public void LoadProgress(PlayerProgress progress) {
      if (progress.KillData.ClearedSpawners.Contains(_id)) {
        Slain = true;
      } else {
        Spawn();
      }
    }

    public void UpdateProgress(PlayerProgress progress) {
      if (Slain) {
        progress.KillData.ClearedSpawners.Add(_id);
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