using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.StaticData {
  public class StaticDataService : IStaticDataService {
    private Dictionary<MonsterId, MonsterStaticData> _monsters;

    public void LoadMonsters() {
      _monsters = Resources.LoadAll<MonsterStaticData>("StaticData/Monster/").ToDictionary(x => x.MonsterId, x => x);
    }

    public MonsterStaticData ForMonster(MonsterId typeId) {
      return _monsters.TryGetValue(typeId, out MonsterStaticData staticData) ? staticData : null;
    }
  }
}