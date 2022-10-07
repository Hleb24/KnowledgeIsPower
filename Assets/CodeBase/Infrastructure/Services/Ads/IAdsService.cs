using System;
using CodeBase.Data;

namespace CodeBase.Infrastructure.Services.Ads {
  public interface IAdsService: IService {
    event Action RewardedVideoReady;
    int Reward { get; }
    bool IsRewardedVideoReady();
    void ShowRewardedVideo(Action onVideoFinished);
    void Initialize();
  }
}