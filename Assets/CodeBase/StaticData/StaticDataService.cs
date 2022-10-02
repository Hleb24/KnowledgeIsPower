using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData {
  public class StaticDataService : IStaticDataService {
    private Dictionary<MonsterId, MonsterStaticData> _monsters;
    private Dictionary<string, LevelStaticData> _levels;

    public void LoadMonsters() {
      _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monster/").ToDictionary(x => x.MonsterId, x => x);
      _levels = Resources.LoadAll<LevelStaticData>("StaticData/Levels/").ToDictionary(x => x.LevelKey, x => x);
    }

    public MonsterStaticData ForMonster(MonsterId typeId) {
      return _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;
    }
  
    public LevelStaticData ForLevel(string sceneKey) {
      return _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;

    }
  }
}