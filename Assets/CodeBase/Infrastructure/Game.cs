using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.States;
using CodeBase.Logic;

namespace CodeBase.Infrastructure {
  public class Game {
    public readonly GameStateMachine stateMachine;

    public Game(ICoroutineRunner coroutineRunner, LoadingCurtain loadingCurtain) {
      stateMachine = new GameStateMachine(new SceneLoader(coroutineRunner), loadingCurtain, AllServices.Container);
    }
  }
}