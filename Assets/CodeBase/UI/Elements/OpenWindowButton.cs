using CodeBase.UI.Services.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Elements {
  public class OpenWindowButton : MonoBehaviour {
    public Button Button;
    public WindowId WindowId;
    private IWindowService _windowService;

    private void Awake() {
      Button.onClick.AddListener(Open);
    }

    public void Construct(IWindowService windowService) {
      _windowService = windowService;
    }

    private void Open() {
      _windowService.Open(WindowId);
    }
  }
}