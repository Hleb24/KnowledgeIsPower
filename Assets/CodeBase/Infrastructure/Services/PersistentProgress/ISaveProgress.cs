using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress {
  public interface ISaveProgressReader {
    void LoadProgress(PlayerProgress progress);
  }

  public interface ISaveProgress : ISaveProgressReader {
    void UpdateProgress(PlayerProgress progress);
  }
}