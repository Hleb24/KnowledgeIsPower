﻿using System;
using System.Collections.Generic;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Logic;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;

namespace CodeBase.Infrastructure.States {
  public class GameStateMachine : IGameStateMachine {
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(SceneLoader sceneLoader, LoadingCurtain loadingCurtain, AllServices services) {
      _states = new Dictionary<Type, IExitableState> {
        { typeof(BootstrapState), new BootstrapState(this, sceneLoader, services) },
        { typeof(LoadProgressState), new LoadProgressState(this, services.Single<IPersistentProgressService>(), services.Single<IStateLoadService>()) },
        { typeof(LoadLevelState), new LoadLevelState(this, sceneLoader, loadingCurtain, services.Single<IGameFactory>(), services.Single<IPersistentProgressService>(), services.Single<IStaticDataService>(), services.Single<IUIFactory>())},
        { typeof(GameLoopState), new GameLoopState(this) }
      };
    }

    public void Enter<TState>() where TState : class, IState {
      IState state = ChangeState<TState>();
      state.Enter();
    }

    public void Enter<TState, TPayload>(TPayload payLoad) where TState : class, IPayLoadedState<TPayload> {
      var state = ChangeState<TState>();
      state.Enter(payLoad);
    }

    private TState ChangeState<TState>() where TState : class, IExitableState {
      _activeState?.Exit();
      var state = GetState<TState>();
      _activeState = state;
      return state;
    }

    private TState GetState<TState>() where TState : class, IExitableState {
      return _states[typeof(TState)] as TState;
    }
  }
}