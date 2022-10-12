using System.Threading.Tasks;
using CodeBase.Infrastructure.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement {
  public interface IAssetProvider : IService {
    Task<GameObject> Instantiate(string address, Vector3 at);
    Task<GameObject> Instantiate(string address);
    Task<T> Load<T>(AssetReference assetReference)  where T : class;
    void Cleanup();
    Task<T> Load<T>(string address) where T : class;
    void Initialize();
  }
}