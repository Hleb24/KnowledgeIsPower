using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using UnityEngine;

namespace CodeBase.UI.Services.Factory {
  public class UIFactory : IUIFactory {
    private const string UI_ROOT_PATH = "UI/UIRoot";
    private readonly IAssetProvider _assets;
    private Transform _uiRoot;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    
    public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService) {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
    }

    public void CreateShop() {
      WindowConfig config = _staticData.ForWindow(WindowId.Shop);
      WindowBase windowBase = Object.Instantiate(config.Prefab, _uiRoot);
      windowBase.Construct(_progressService);
    }

    public void CreateUIRoot() {
      _uiRoot = _assets.Instantiate(UI_ROOT_PATH).transform;
    }
  }
}