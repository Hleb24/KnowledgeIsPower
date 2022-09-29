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
      if (_saveLoadService.LoadProgress() == null) {
        _progressService.Progress = NewProgress();
        Debug.Log("LoadProgress is null");
      } else {
        _progressService.Progress = _saveLoadService.LoadProgress();
      }
    }

    private PlayerProgress NewProgress() {
      Debug.Log("NewProgress");
      var playerProgress = new PlayerProgress(BootstrapState.MAIN);
      playerProgress.HeroState.MaxHP = 50;
      playerProgress.HeroState.ResetHP();
      playerProgress.HeroStats.Damage = 1.0f;
      playerProgress.HeroStats.DamageRadius = .5f;
      return playerProgress;
    }
  }
}