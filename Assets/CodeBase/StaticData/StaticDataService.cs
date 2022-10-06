using System.Collections.Generic;
using System.Linq;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.StaticData {
  public class StaticDataService : IStaticDataService {
    private const string STATIC_DATA_MONSTER = "StaticData/Monster/";
    private const string STATIC_DATA_LEVELS = "StaticData/Levels/";
    private const string STATIC_DATA_WINDOWS = "StaticData/UI/WindowStaticData";
    private Dictionary<MonsterId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId,WindowConfig> _windowConfigs;

    public void LoadMonsters() {
      _monsters = Resources.LoadAll<MonsterStaticData>(STATIC_DATA_MONSTER).ToDictionary(x => x.MonsterId, x => x);
      _levels = Resources.LoadAll<LevelStaticData>(STATIC_DATA_LEVELS).ToDictionary(x => x.LevelKey, x => x);
      _windowConfigs = Resources.Load<WindowsStaticData>(STATIC_DATA_WINDOWS).WindowConfigs.ToDictionary(x => x.WindowId, x => x);
    }

    public MonsterStaticData ForMonster(MonsterId typeId) {
      return _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;
    }
  
    public LevelStaticData ForLevel(string sceneKey) {
      return _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;

    }

    public WindowConfig ForWindow(WindowId windowId) {
     return _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig) ? windowConfig : null;
    }
  }
}