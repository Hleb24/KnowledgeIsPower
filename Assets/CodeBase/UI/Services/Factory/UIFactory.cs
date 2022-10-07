using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory {
  public class UIFactory : IUIFactory {
    private const string UI_ROOT_PATH = "UI/UIRoot";
    private readonly IAssetProvider _assets;
    private Transform _uiRoot;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly IAdsService _adsService;

    public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adsService) {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
      _adsService = adsService;
    }

    public void CreateShop() {
      WindowConfig config = _staticData.ForWindow(WindowId.Shop);
      ShopWindow shopWindow = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
      shopWindow.Construct(_adsService, _progressService);
    }

    public void CreateUIRoot() {
      _uiRoot = _assets.Instantiate(UI_ROOT_PATH).transform;
    }
  }
}