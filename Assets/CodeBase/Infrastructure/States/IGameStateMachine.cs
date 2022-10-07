using CodeBase.Infrastructure.Services;

namespace CodeBase.Infrastructure.States {
  public interface IGameStateMachine: IService {
    void Enter<TState>() where TState : class, IState;
    void Enter<TState, TPayload>(TPayload payLoad) where TState : class, IPayLoadedState<TPayload>;
  }
}