using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory {
  public sealed class GameFactory : IGameFactory {
    private readonly IAssetProvider _assetsProvider;
    private readonly IStaticDataService _staticData;

    public GameFactory(IAssetProvider assetsProvider, IStaticDataService staticData) {
      _assetsProvider = assetsProvider;
      _staticData = staticData;
    }

    public GameObject CreateHud() {
      return InstantiateRegistered(AssetPath.HUD_PATH);
    }

    public GameObject CreateHero(GameObject at) {
      HeroGameObject = InstantiateRegistered(AssetPath.HERO_PATH, at.transform.position);
      return HeroGameObject;
    }

    public void CleanUp() {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
    }

    public void Register(ISaveProgressReader progressReader) {
      if (progressReader is ISaveProgress progressWriter) {
        ProgressWriters.Add(progressWriter);
      }

      ProgressReaders.Add(progressReader);
    }

    public GameObject InstantiateMonster(MonsterId monsterTypeId, Transform parent) {
      MonsterStaticData monsterData = _staticData.ForMonster(monsterTypeId);
      GameObject monster = Object.Instantiate(monsterData.Prefab, parent.position, Quaternion.identity, parent);
      var health = monster.GetComponent<IHealth>();
      health.Current = monsterData.HP;
      health.Max = monsterData.HP;

      monster.GetComponent<ActionUI>().Construct(health);
      monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      var attack = monster.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;

      monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

      return monster;
    }

    public void Unregister(EnemySpawner enemySpawner) {
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 position) {
      GameObject gameObject = _assetsProvider.Instantiate(prefabPath, position);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath) {
      GameObject gameObject = _assetsProvider.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject) {
      foreach (ISaveProgressReader progressReader in gameObject.GetComponentsInChildren<ISaveProgressReader>()) {
        Register(progressReader);
      }
    }

    public List<ISaveProgressReader> ProgressReaders { get; } = new();
    public List<ISaveProgress> ProgressWriters { get; } = new();

    public GameObject HeroGameObject { get; private set; }
  }
}