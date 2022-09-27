using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
  public class LoadProgressState : IState {
    private readonly GameStateMachine _gameStateMachine;
    private readonly IPersistentProgressService _progressService;
    private readonly IStateLoadService _saveLoadService;

    public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, IStateLoadService saveLoadService) {
      _gameStateMachine = gameStateMachine;
      _progressService = progressService;
      _saveLoadService = saveLoadService;
    }

    public void Enter() {
      LoadProgressOrInitNew();
      _gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
    }

    public void Exit() { }

    private void LoadProgressOrInitNew() {
      _progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
    }

    private PlayerProgress NewProgress() {
      Debug.Log("NewProgress");
      return new PlayerProgress(BootstrapState.MAIN);
    }
  }
}