using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory {
  public sealed class GameFactory : IGameFactory {
    private readonly IAssetProvider _assetsProvider;
    public event Action HeroCreated;

    public GameFactory(IAssetProvider assetsProvider) {
      _assetsProvider = assetsProvider;
    }

    public GameObject CreateHud() {
      return InstantiateRegistered(AssetPath.HUD_PATH);
    }

    public GameObject CreateHero(GameObject at) {
      HeroGameObject = InstantiateRegistered(AssetPath.HERO_PATH, at.transform.position);
      HeroCreated?.Invoke();
      return HeroGameObject;
    }

    public void CleanUp() {
      ProgressReaders.Clear();
      ProgressWriters.Clear();
    }

    private GameObject InstantiateRegistered(string prefabPath, Vector3 position) {
      GameObject gameObject = _assetsProvider.Instantiate(prefabPath, position);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private GameObject InstantiateRegistered(string prefabPath) {
      GameObject gameObject = _assetsProvider.Instantiate(prefabPath);
      RegisterProgressWatchers(gameObject);
      return gameObject;
    }

    private void RegisterProgressWatchers(GameObject gameObject) {
      foreach (ISaveProgressReader progressReader in gameObject.GetComponentsInChildren<ISaveProgressReader>()) {
        Register(progressReader);
      }
    }

    private void Register(ISaveProgressReader progressReader) {
      if (progressReader is ISaveProgress progressWriter) {
        ProgressWriters.Add(progressWriter);
      }

      ProgressReaders.Add(progressReader);
    }

    public GameObject HeroGameObject { get; private set; }

    public List<ISaveProgressReader> ProgressReaders { get; } = new();
    public List<ISaveProgress> ProgressWriters { get; } = new();
  }
}