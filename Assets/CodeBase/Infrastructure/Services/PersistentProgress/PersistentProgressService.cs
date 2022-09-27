using System;
using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.PersistentProgress {
  [Serializable]
  public class PersistentProgressService : IPersistentProgressService {
    public PlayerProgress Progress { get; set; }
  }
}