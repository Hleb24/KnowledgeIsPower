using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Services.Randomizer;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy {
  public class LootsSpawner : MonoBehaviour {
 
    
    public EnemyDeath EnemyDeath;
    private IGameFactory _factory;
    private IRandomService _randomService;
    private int _lootMax;
    private int _lootMin;

    private void Start() {
      EnemyDeath.Happened += OnHappened;
    }

    public void Construct(IGameFactory gameFactory, IRandomService randomService) {
      _factory = gameFactory;
      _randomService = randomService;
    }

    public void SetLoot(int min, int max) {
      _lootMin = min;
      _lootMax = max;
    }

    private void OnHappened() {
      LootPiece lootPiece = _factory.CreateLoot();
      lootPiece.transform.position = transform.position;

      Loot lootItem = GenerateLootItem();
      lootPiece.Initialize(lootItem);
    }

    private Loot GenerateLootItem() {
      var lootItem = new Loot {
        Value = _randomService.Next(_lootMin, _lootMax)
      };
      return lootItem;
    }
  }
}