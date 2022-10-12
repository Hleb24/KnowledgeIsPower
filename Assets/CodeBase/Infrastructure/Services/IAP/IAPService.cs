using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP {
  public class IAPService : IIAPService {
    private static bool ProductBoughtOut(bool notBought, BoughtIAP boughtIAP, ProductConfig config) {
      return notBought && boughtIAP.Count >= config.MaxPurchaseCount;
    }

    private readonly IAPProvider _provider;
    private readonly IPersistentProgressService _progressService;
    public event Action Initialized;

    public IAPService(IAPProvider provider, IPersistentProgressService progressService) {
      _provider = provider;
      _progressService = progressService;
    }

    public void Initialize() {
      _provider.Initialize(this);
      _provider.Initialized += () => Initialized?.Invoke();
    }

    public void StartPurchase(string productId) {
      _provider.StartPurchase(productId);
    }

    public List<ProductDescription> Products() {
      return ProductDescriptions().ToList();
    }

    public PurchaseProcessingResult ProcessPurchase(Product purchaseProduct) {
      ProductConfig productConfig = _provider.Configs[purchaseProduct.definition.id];
      switch (productConfig.ItemType) {
        case ItemType.Skulls:
          _progressService.Progress.WorldData.LootData.Add(productConfig.Quantity);
          _progressService.Progress.PurchaseData.AddPurchase(purchaseProduct.definition.id);
          break;
      }

      return PurchaseProcessingResult.Complete;
    }

    private IEnumerable<ProductDescription> ProductDescriptions() {
      PurchaseData purchaseData = _progressService.Progress.PurchaseData;
      foreach (string productId in _provider.Products.Keys) {
        ProductConfig config = _provider.Configs[productId];
        Product product = _provider.Products[productId];
        BoughtIAP boughtIAP = purchaseData.BoughtIAPs.Find(x => x.IAPId == productId);
        bool notBought = boughtIAP != null;
        if (ProductBoughtOut(notBought, boughtIAP, config)) {
          continue;
        }

        yield return new ProductDescription {
          Id = productId, ProductConfig = config, Product = product,
          AvailablePurchasesLeft = notBought ? config.MaxPurchaseCount - boughtIAP.Count : config.MaxPurchaseCount
        };
      }
    }

    public bool IsInitialize {
      get {
        return _provider.IsInitialize;
      }
    }
  }
}