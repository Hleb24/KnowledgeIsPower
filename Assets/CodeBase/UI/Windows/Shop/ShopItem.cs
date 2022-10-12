using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.IAP;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop {
  public class ShopItem : MonoBehaviour {
    public Button BuyItemButton;
    public TextMeshProUGUI PriceText;
    public TextMeshProUGUI QuantityText;
    public TextMeshProUGUI AvailableItemText;
    public Image Icon;
    
    private ProductDescription _productDescription;
    private IIAPService _iapService;
    private IAssetProvider _assets;

    public void Construct(IIAPService iapService, IAssetProvider assets, ProductDescription description) {
      _productDescription = description;
      _iapService = iapService;
      _assets = assets;
    }

    public async void Initialize() {
      BuyItemButton.onClick.AddListener(OnBuyItemClick);
      PriceText.text = _productDescription.ProductConfig.Price;
      QuantityText.text = _productDescription.ProductConfig.Quantity.ToString();
      AvailableItemText.text = _productDescription.AvailablePurchasesLeft.ToString();
      Icon.sprite = await _assets.Load<Sprite>(_productDescription.ProductConfig.Icon);
    }

    private void OnBuyItemClick() {
      _iapService.StartPurchase(_productDescription.Id);
    }
  }
}