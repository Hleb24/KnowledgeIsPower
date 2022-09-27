using CodeBase.Cameralogic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
  public class LoadLevelState : IPayLoadedState<string> {
    private const string INITIAL_POINT_TAG = "InitialPoint";
    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly LoadingCurtain _loadingCurtain;
    private readonly IGameFactory _gameFactory;
    private readonly IPersistentProgressService _progressService;

    public LoadLevelState(GameStateMachine stateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory,
      IPersistentProgressService progressService) {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _loadingCurtain = loadingCurtain;
      _gameFactory = gameFactory;
      _progressService = progressService;
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
      foreach (ISaveProgress progressReader in _gameFactory.ProgressWriters) {
        progressReader.UpdateProgress(_progressService.Progress);
      }
    }

    private void InitGameWorld() {
      GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(INITIAL_POINT_TAG));
      _gameFactory.CreateHud();
      CameraFollow(hero);
    }

    private void CameraFollow(GameObject hero) {
      Camera.main.GetComponent<CameraFollow>().Follow(hero);
    }
  }
}