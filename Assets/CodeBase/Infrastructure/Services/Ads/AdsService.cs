using System;
using UnityEngine;
using UnityEngine.Advertisements;

namespace CodeBase.Infrastructure.Services.Ads {
  public class AdsService : IUnityAdsListener, IAdsService {
    private const string ANDROID_GAME_ID = "4960719";
    private const string IOS_GAME_ID = "4960718";
    private const string REWARDED_VIDEO_PLACEMENT_ID = "rewardedVideo";
    public event Action RewardedVideoReady;
    private string _gameId;
    private Action _onVideoFinished;

    public bool IsRewardedVideoReady() {
      return Advertisement.IsReady(REWARDED_VIDEO_PLACEMENT_ID);
    }

    public void ShowRewardedVideo(Action onVideoFinished) {
      Advertisement.Show(REWARDED_VIDEO_PLACEMENT_ID);
      _onVideoFinished = onVideoFinished;
    }

    public void Initialize() {
      Advertisement.AddListener(this);
      switch (Application.platform) {
        case RuntimePlatform.Android:
          _gameId = ANDROID_GAME_ID;
          break;
        case RuntimePlatform.IPhonePlayer:
          _gameId = IOS_GAME_ID;
          break;
        case RuntimePlatform.WindowsEditor:
          _gameId = ANDROID_GAME_ID;
          break;
        default:
          Debug.Log("Unsupported platform for ads");
          break;
      }

      Advertisement.Initialize(_gameId);
    }

    public void OnUnityAdsReady(string placementId) {
      Debug.Log("OnUnityAdsReady " + placementId);
      if (placementId == REWARDED_VIDEO_PLACEMENT_ID) {
        RewardedVideoReady?.Invoke();
      }
    }

    public void OnUnityAdsDidError(string message) {
      Debug.Log("OnUnityAdsDidError" + message);
    }

    public void OnUnityAdsDidStart(string placementId) {
      Debug.Log("OnUnityAdsDidStart " + placementId);
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult) {
      switch (showResult) {
        case ShowResult.Failed:
          Debug.LogError("OnUnityAdsDidFinish " + showResult);
          break;
        case ShowResult.Skipped:
          Debug.LogError("OnUnityAdsDidFinish " + showResult);
          break;
        case ShowResult.Finished:
          _onVideoFinished?.Invoke();
          break;
        default:
          Debug.LogError("OnUnityAdsDidFinish " + showResult);
          break;
      }

      _onVideoFinished = null;
    }

    public int Reward { get; } = 13;
  }
}