﻿using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.Hero {
  public class HpBar : MonoBehaviour {
    public Image ImageCurrent;
    public void SetValue(float current, float max) {
      Debug.Log($"SetValue {current}, {max}");
      ImageCurrent.fillAmount = current / max;
    }
  }
}