using System.Collections.Generic;
using CodeBase.Enemy;
using CodeBase.Hero;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using CodeBase.UI;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory {
  public sealed class GameFactory : IGameFactory {
    private readonly IAssetProvider _assetsProvider;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _progressService;

    public GameFactory(IAssetProvider assetsProvider, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService) {
      _assetsProvider = assetsProvider;
      _staticData = staticData;
      _randomService = randomService;
      _progressService = progressService;
    }

    public GameObject CreateHud() {
      GameObject hud = InstantiateRegistered(AssetPath.HUD_PATH);
      hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
      return hud;
    }

    public GameObject CreateHero(GameObject at) {
      HeroGameObject = InstantiateRegistered(AssetPath.HERO_PATH, at.transform.position);
      return HeroGameObject;
    }

    public LootPiece CreateLoot() {
      var lootPiece = InstantiateRegistered(AssetPath.LOOT_PATH).GetComponent<LootPiece>();
      lootPiece.Construct(_progressService.Progress.WorldData);
      return lootPiece;
    }

    public void CreateSpawner(Vector3 at, string spawnerId, MonsterId monsterId) {
      var spawner = InstantiateRegistered(AssetPath.SPAWNER, at).GetComponent<SpawnPoint>();
      spawner.Construct(this);
      spawner.ID = spawnerId;
      spawner.MonsterTypeId = monsterId;
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

      monster.GetComponent<ActorUI>().Construct(health);
      monster.GetComponent<AgentMoveToPlayer>().Construct(HeroGameObject.transform);
      monster.GetComponent<NavMeshAgent>().speed = monsterData.MoveSpeed;

      var lootSpawner = monster.GetComponentInChildren<LootsSpawner>();
      lootSpawner.Construct(this, _randomService);
      lootSpawner.SetLoot(monsterData.MinLoot, monsterData.MaxLoot);
      

      var attack = monster.GetComponent<Attack>();
      attack.Construct(HeroGameObject.transform);
      attack.Damage = monsterData.Damage;
      attack.Cleavage = monsterData.Cleavage;
      attack.EffectiveDistance = monsterData.EffectiveDistance;

      monster.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);

      return monster;
    }

    public void Unregister(SpawnPoint spawnPoint) { }

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