using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.Windows.Shop {
  public class ShopWindow : WindowBase {
    public TextMeshProUGUI SkullText;
    public RewardedAdItem AdItem;
    public ShopItemsContainer ShopItemsContainer;
    
    public void Construct(IAdsService adsService, IPersistentProgressService progress, IIAPService iapService, IAssetProvider assetsProvider){
      base.Construct(progress);
      AdItem.Construct(adsService, progress);
      ShopItemsContainer.Construct(iapService, progress, assetsProvider);
    }

    protected override void Initialize() {
      AdItem.Initialize();
      ShopItemsContainer.Initialize();

      RefreshSkullText();
    }

    protected override void SubscribeUpdates() {
      AdItem.Subscribe();
      ShopItemsContainer.Subscribe();

      Progress.WorldData.LootData.Changed += RefreshSkullText;
    }

    protected override void Cleanup() {
      base.Cleanup();
      AdItem.Cleanup();
      ShopItemsContainer.Cleanup();

      Progress.WorldData.LootData.Changed -= RefreshSkullText;
    }

    private void RefreshSkullText() {
      SkullText.text = Progress.WorldData.LootData.Collected.ToString();
    }
  }
}