using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using CodeBase.UI.Windows;
using CodeBase.UI.Windows.Shop;
using UnityEngine;

namespace CodeBase.UI.Services.Factory {
  public class UIFactory : IUIFactory {
    private const string UI_ROOT_PATH = "UIRoot";
    private readonly IAssetProvider _assets;
    private readonly IStaticDataService _staticData;
    private readonly IPersistentProgressService _progressService;
    private readonly IAdsService _adsService;
    private readonly IIAPService _aipService;
    private Transform _uiRoot;

    public UIFactory(IAssetProvider assets, IStaticDataService staticData, IPersistentProgressService progressService, IAdsService adsService, IIAPService aipService) {
      _assets = assets;
      _staticData = staticData;
      _progressService = progressService;
      _adsService = adsService;
      _aipService = aipService;
    }

    public void CreateShop() {
      WindowConfig config = _staticData.ForWindow(WindowId.Shop);
      ShopWindow shopWindow = Object.Instantiate(config.Prefab, _uiRoot) as ShopWindow;
      shopWindow.Construct(_adsService, _progressService, _aipService, _assets);
    }

    public async Task CreateUIRoot() {
      GameObject instantiate = await _assets.Instantiate(UI_ROOT_PATH);
      _uiRoot = instantiate.transform;
    }
  }
}