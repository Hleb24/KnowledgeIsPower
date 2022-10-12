using System;
using System.Collections.Generic;

namespace CodeBase.Data {
  [Serializable]
  public class PurchaseData {
    public List<BoughtIAP> BoughtIAPs = new();
    public Action Changed;

    public void AddPurchase(string id) {
      BoughtIAP boughtIAP = Product(id);
      if (boughtIAP != null) {
        boughtIAP.Count++;
      } else {
        BoughtIAPs.Add(new BoughtIAP { IAPId = id, Count = 1 });
      }
      Changed?.Invoke();
    }

    private BoughtIAP Product(string id) {
      return BoughtIAPs.Find(x => x.IAPId == id);
    }
  }
}