using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Infrastructure.Factory {
  public interface IGameFactory : IService {
    GameObject HeroGameObject { get;}
    event Action HeroCreated;
    GameObject CreateHud();
    GameObject CreateHero(GameObject at);
    void CleanUp();
    List<ISaveProgressReader> ProgressReaders { get; }
    List<ISaveProgress> ProgressWriters { get; }
  }
}