namespace CodeBase.Infrastructure.States {
  public interface IState : IExitableState {
    void Enter();
  }

  public interface IPayLoadedState<TPayload> : IExitableState {
    void Enter(TPayload payLoad);
  }

  public interface IExitableState {
    void Exit();
  }
}