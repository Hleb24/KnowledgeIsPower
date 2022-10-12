using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.AI;

namespace CodeBase.Infrastructure.Factory {
  public sealed class GameFactory : IGameFactory {
    private readonly IAssetProvider _assetsProvider;
    private readonly IStaticDataService _staticData;
    private readonly IRandomService _randomService;
    private readonly IPersistentProgressService _progressService;
    private readonly IWindowService _windowService;

    public GameFactory(IAssetProvider assetsProvider, IStaticDataService staticData, IRandomService randomService, IPersistentProgressService progressService,
      IWindowService windowService) {
      _assetsProvider = assetsProvider;
      _staticData = staticData;
      _randomService = randomService;
      _progressService = progressService;
      _windowService = windowService;
    }

    public async Task<GameObject> CreateHud() {
      GameObject hud = await InstantiateRegistered(AssetsAddress.HUD_PATH);
      hud.GetComponentInChildren<LootCounter>().Construct(_progressService.Progress.WorldData);
      foreach (OpenWindowButton openWindowButton in hud.GetComponentsInChildren<OpenWindowButton>()) {
        openWindowButton.Construct(_windowService);
      }

      return hud;
    }

    public async Task<GameObject> CreateHero(LevelStaticData at) {
      HeroGameObject = await InstantiateRegistered(AssetsAddress.HERO_PATH, at.InitialHeroPosition);
      return HeroGameObject;
    }

    public async Task<LootPiece> CreateLoot() {
      var prefab = await _assetsProvider.Load<GameObject>(AssetsAddress.LOOT_PATH);

      LootPiece lootPiece = InstantiateRegistered(prefab).GetComponent<LootPiece>();
      lootPiece.Construct(_progressService.Progress.WorldData);
      return lootPiece;
    }

    public async Task CreateSpawner(Vector3 at, string spawnerId, MonsterId monsterId) {
      var prefab = await _assetsProvider.Load<GameObject>(AssetsAddress.SPAWNER);
      var spawner = InstantiateRegistered(prefab, at).GetComponent<SpawnPoint>();
      spawner.Construct(this);
      spawner.ID = spawnerId;
      spawner.MonsterTypeId = monsterId;
    }

    public void CleanUp() {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
      _assetsProvider.Cleanup();
    }

    public async Task WarmUp() {
      await _assetsProvider.Load<GameObject>(AssetsAddress.LOOT_PATH);
      await _assetsProvider.Load<GameObject>(AssetsAddress.SPAWNER);
    }

    public async Task<GameObject> InstantiateMonster(MonsterId monsterTypeId, Transform parent) {
      MonsterStaticData monsterData = _staticData.ForMonster(monsterTypeId);

      var prefab = await _assetsProvider.Load<GameObject>(monsterData.PrefabReference);

      GameObject monster = Object.Instantiate(prefab, parent.position, Quaternion.identity, parent);
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

    private void Register(ISaveProgressReader progressReader) {
      if (progressReader is ISaveProgress progressWriter) {
        ProgressWriters.Add(progressWriter);
      }

      ProgressReaders.Add(progressReader);
    }

    private GameObject InstantiateRegistered(GameObject prefab, Vector3 at) {
      GameObject gameObject = Object.Instantiate(prefab, at, Quaternion.identity);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }
    
    private GameObject InstantiateRegistered(GameObject prefab) {
      GameObject gameObject =  Object.Instantiate(prefab);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private async Task<GameObject> InstantiateRegistered(string prefabPath, Vector3 at) {
      GameObject gameObject = await _assetsProvider.Instantiate(prefabPath, at);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private async Task<GameObject> InstantiateRegistered(string prefabPath) {
      GameObject gameObject = await  _assetsProvider.Instantiate(prefabPath);
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