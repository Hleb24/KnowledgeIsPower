using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.SaveLoad {
  public interface IStateLoadService : IService {
    void SaveProgress();
    PlayerProgress LoadProgress();
  }
}