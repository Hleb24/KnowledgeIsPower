﻿using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Input;
using UnityEngine;

namespace CodeBase.Infrastructure.States {
  public class BootstrapState : IState {
    public const string MAIN = "Main";
    private const string INITIAL = "Initial";

    private readonly GameStateMachine _stateMachine;
    private readonly SceneLoader _sceneLoader;
    private readonly AllServices _services;

    public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices allServices) {
      _stateMachine = stateMachine;
      _sceneLoader = sceneLoader;
      _services = allServices;

      RegisterServices();
    }

    public void Enter() {
      _sceneLoader.Load(INITIAL, EnterLoadLevel);
    }

    public void Exit() { }

    private IInputService RegisterInputService() {
      return Application.isEditor ? new StandaloneInputService() : new MobileInputService();
    }

    private void EnterLoadLevel() {
      _stateMachine.Enter<LoadProgressState>();
    }

    private void RegisterServices() {
      _services.RegisterSingle(RegisterInputService());
      _services.RegisterSingle<IAssetProvider>(new AssetProvider());
      _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
      _services.RegisterSingle<IGameFactory>(new GameFactory(_services.Single<IAssetProvider>()));
      _services.RegisterSingle<IStateLoadService>(new StateLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
    }
  }
}