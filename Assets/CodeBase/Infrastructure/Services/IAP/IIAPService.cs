﻿using System;
using System.Collections.Generic;

namespace CodeBase.Infrastructure.Services.IAP {
  public interface IIAPService : IService {
    event Action Initialized;
    void Initialize();
    List<ProductDescription> Products();
    void StartPurchase(string productId);
  }
}