using CodeBase.Infrastructure.Services;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;

namespace CodeBase.StaticData {
  public interface IStaticDataService : IService {
    void LoadMonsters();
    MonsterStaticData ForMonster(MonsterId typeId);
    LevelStaticData ForLevel(string sceneKey);
    WindowConfig ForWindow(WindowId windowId);
  }
}