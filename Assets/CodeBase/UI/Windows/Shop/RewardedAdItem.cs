using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Windows.Shop {
  public class RewardedAdItem : MonoBehaviour {
    public Button ShowAdButton;
    public GameObject[] AdActiveObjects;
    public GameObject[] AdInactiveObjects;
    private IAdsService _adsService;
    private IPersistentProgressService _progressService;

    public void Construct(IAdsService adsService, IPersistentProgressService progressService) {
      _adsService = adsService;
      _progressService = progressService;
    }

    public void Initialize() {
      ShowAdButton.onClick.AddListener(OnShowAdClick);
      RefreshAvailableAd();
    }

    public void Subscribe() {
      _adsService.RewardedVideoReady += OnRewardedVideoReady;
    }

    public void Cleanup() {
      _adsService.RewardedVideoReady -= OnRewardedVideoReady;
    }

    private void OnRewardedVideoReady() {
      RefreshAvailableAd();
    }

    private void RefreshAvailableAd() {
      bool isRewardedVideoReady = _adsService.IsRewardedVideoReady();
      foreach (GameObject activeObject in AdActiveObjects) {
        activeObject.SetActive(isRewardedVideoReady);
      }
      
      foreach (GameObject inactiveObject in AdInactiveObjects) {
        inactiveObject.SetActive(!isRewardedVideoReady);
      }
    }

    private void OnShowAdClick() {
      _adsService.ShowRewardedVideo(OnVideoFinished);
    }

    private void OnVideoFinished() {
      _progressService.Progress.WorldData.LootData.Add(_adsService.Reward);
    }
  }
}