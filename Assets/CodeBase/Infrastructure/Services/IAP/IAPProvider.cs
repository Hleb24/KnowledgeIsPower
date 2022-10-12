using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using UnityEngine;
using UnityEngine.Purchasing;

namespace CodeBase.Infrastructure.Services.IAP {
  public class IAPProvider : IStoreListener {
    private const string IAP_CONFIGS_PATH = "products";
    public event Action Initialized;
    private IExtensionProvider _extensions;
    private IStoreController _controller;
    private IAPService _iapService;

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
      _controller = controller;
      _extensions = extensions;
      foreach (Product product in _controller.products.all) {
        Products.Add(product.definition.id, product);
      }
      Initialized?.Invoke();
      Debug.Log("UnityPurchasing initialized success");
    }

    public void OnInitializeFailed(InitializationFailureReason error) {
      Debug.Log($"UnityPurchasing initialized failed {error}");
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent) {
      Debug.Log($"ProcessPurchase success {purchaseEvent.purchasedProduct.definition.id}");

      return _iapService.ProcessPurchase(purchaseEvent.purchasedProduct);
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
      Debug.Log($"Product {product.definition.id} purchase failed {failureReason} transaction id {product.transactionID}");
    }

    public void Initialize(IAPService iapService) {
      _iapService = iapService;
      Configs = new Dictionary<string, ProductConfig>();
      Products = new Dictionary<string, Product>();
      Load();
      var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
      foreach (ProductConfig config in Configs.Values) {
        builder.AddProduct(config.Id, config.ProductType);
      }

      UnityPurchasing.Initialize(this, builder);
    }

    public void StartPurchase(string productId) {
      _controller.InitiatePurchase(productId);
    }

    private void Load() {
      Configs = Resources.Load<TextAsset>(IAP_CONFIGS_PATH).text.ToDeserialized<ProductConfigWrapper>().Configs.ToDictionary(x => x.Id, x => x);
    }

    public Dictionary<string, ProductConfig> Configs { get; private set; }
    public Dictionary<string, Product> Products { get; private set; }

    public bool IsInitialize {
      get {
        return _controller != null && _extensions != null;
      }
    }
  }
}