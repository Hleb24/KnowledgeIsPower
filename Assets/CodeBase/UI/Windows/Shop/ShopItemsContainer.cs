using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.UI.Windows.Shop {
  public class ShopItemsContainer : MonoBehaviour {
    private const string SHOP_ITEM_PATH = "ShopItem";
    private readonly List<GameObject> _shopItems = new();
    public GameObject[] ShopUnavailableObjects;
    public Transform Parent;
    private IIAPService _iapService;
    private IPersistentProgressService _progressService;
    private IAssetProvider _assetProvider;
 
    public void Construct(IIAPService iapService, IPersistentProgressService progress, IAssetProvider assetProvider) {
      _iapService = iapService;
      _progressService = progress;
      _assetProvider = assetProvider;
    }

    public void Initialize() {
      RefreshAvailableItems();
    }

    public void Subscribe() {
      _iapService.Initialized += RefreshAvailableItems;
      _progressService.Progress.PurchaseData.Changed += RefreshAvailableItems;
    }

    public void Cleanup() {
      _iapService.Initialized -= RefreshAvailableItems;
      _progressService.Progress.PurchaseData.Changed -= RefreshAvailableItems;
    }

    private async void RefreshAvailableItems() {
      UpdateUnavailableObjects();

      if (!_iapService.IsInitialize) {
        return;
      }

      ClearShopItems();

      await FillShopItems();
    }

    private void ClearShopItems() {
      foreach (GameObject shopItem in _shopItems) {
        Destroy(shopItem.gameObject);
      }
    }

    
    private async Task FillShopItems() {
      foreach (ProductDescription productDescription in _iapService.Products()) {
        GameObject instantiate = await _assetProvider.Instantiate(SHOP_ITEM_PATH, Parent);
        var shopItem = instantiate.GetComponent<ShopItem>();

        shopItem.Construct(_iapService, _assetProvider, productDescription);
        shopItem.Initialize();
        _shopItems.Add(instantiate);
      }
    }

    private void UpdateUnavailableObjects() {
      foreach (GameObject o in ShopUnavailableObjects) {
        o.SetActive(!_iapService.IsInitialize);
      }
    }
  }
}