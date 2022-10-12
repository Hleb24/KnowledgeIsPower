using System.Threading.Tasks;
using CodeBase.Cameralogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Elements;
using CodeBase.UI.Services.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Infrastructure.States {
  public class LoadLevelState : IPayLoadedState<string> {
    private static void BindHeroHealth(GameObject hud, GameObject hero) {
      hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<IHealth>());
    }

    private const string INITIAL_POINT_TAG = "InitialPoint";
    private const string ENEMY_SPAWNER_TAG = "EnemySpawner";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;
    private readonly IStaticDataService _staticData;
    private readonly IUIFactory _uiFactory;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory,
      IPersistentProgressService progressService, IStaticDataService staticData, IUIFactory uiFactory) {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
      _uiFactory = uiFactory;
    }

    public void Enter(string sceneName) {
      _loadingCurtain.Show();
      _gameFactory.CleanUp();
      _gameFactory.WarmUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() {
      _loadingCurtain.Hide();
    }

    private async void OnLoaded() {
      await InitUIRoot();
      await InitGameWorld();
      InformProgressReaders();
      _stateMachine.Enter<GameLoopState>();
    }

    private async Task InitUIRoot() {
     await _uiFactory.CreateUIRoot();
    }

    private void InformProgressReaders() {
      foreach (ISaveProgressReader progressReader in _gameFactory.ProgressReaders) {
        progressReader.LoadProgress(_progressService.Progress);
      }
    }

    private async Task InitGameWorld() {
      LevelStaticData levelData = LevelStaticData();
      await InitSpawners(levelData);
      GameObject hero = await _gameFactory.CreateHero(levelData);
      GameObject hud = await _gameFactory.CreateHud();
      BindHeroHealth(hud, hero);

      CameraFollow(hero);
    }

    private LevelStaticData LevelStaticData() {
      string sceneKey = SceneManager.GetActiveScene().name;
      LevelStaticData levelData = _staticData.ForLevel(sceneKey);
      return levelData;
    }

    private async Task InitSpawners(LevelStaticData levelData) {
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners) {
        await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterId);
      }
    }

    private void CameraFollow(GameObject hero) {
      Camera camera = Camera.main;
      if (camera == null) {
        return;
      }

      camera.GetComponent<CameraFollow>().Follow(hero);
    }
  }
}