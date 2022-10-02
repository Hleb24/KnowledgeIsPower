using System.Linq;
using CodeBase.Cameralogic;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI;
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

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory,
      IPersistentProgressService progressService, IStaticDataService staticData) {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
      _staticData = staticData;
    }

    public void Enter(string sceneName) {
      _loadingCurtain.Show();
      _gameFactory.CleanUp();
      _sceneLoader.Load(sceneName, OnLoaded);
    }

    public void Exit() {
      _loadingCurtain.Hide();
    }

    private void OnLoaded() {
      InitGameWorld();
      InformProgressReaders();
      _stateMachine.Enter<GameLoopState>();
    }

    private void InformProgressReaders() {
      foreach (ISaveProgressReader progressReader in _gameFactory.ProgressReaders) {
        progressReader.LoadProgress(_progressService.Progress);
      }
    }

    private void InitGameWorld() {
      InitSpawners();
      GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(INITIAL_POINT_TAG));
      GameObject hud = _gameFactory.CreateHud();
      BindHeroHealth(hud, hero);

      CameraFollow(hero);
    }

    private void InitSpawners() {
      string sceneKey = SceneManager.GetActiveScene().name;
      LevelStaticData levelData = _staticData.ForLevel(sceneKey);
      foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners) {
        _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.Id, spawnerData.MonsterId);
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