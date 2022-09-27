using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Services.SaveLoad {
  public class StateLoadService : IStateLoadService {
    private const string PROGRESS_KEY = "Progress";
    private readonly IPersistentProgressService _progressService;
    private readonly IGameFactory _gameFactory;

    public StateLoadService(IPersistentProgressService progressService, IGameFactory gameFactory) {
      _progressService = progressService;
      _gameFactory = gameFactory;
    }

    public void SaveProgress() {
      foreach (ISaveProgress progressWriter in _gameFactory.ProgressWriters) {
        progressWriter.LoadProgress(_progressService.Progress);
      }

      PlayerPrefs.SetString(PROGRESS_KEY, _progressService.Progress.ToJson());
    }

    public PlayerProgress LoadProgress() {
      return PlayerPrefs.GetString(PROGRESS_KEY)?.ToDeserialized<PlayerProgress>();
    }
  }
}