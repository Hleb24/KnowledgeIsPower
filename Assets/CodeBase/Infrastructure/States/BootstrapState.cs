using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.IAP;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Services.Input;
using CodeBase.Services.Randomizer;
using CodeBase.StaticData;
using CodeBase.UI.Services.Factory;
using CodeBase.UI.Services.Windows;
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
      RegisterAdsService();
      _services.RegisterSingle(RegisterInputService());
      _services.RegisterSingle<IGameStateMachine>(_stateMachine);
      RegisterAssetProvider();
      _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
      
      RegisterIAPService(new IAPProvider(), _services.Single<IPersistentProgressService>());
      
      RegisterStaticDataService();
      _services.RegisterSingle<IRandomService>(new RandomService());
      _services.RegisterSingle<IUIFactory>(
        new UIFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IPersistentProgressService>(),_services.Single<IAdsService>()));
      _services.RegisterSingle<IWindowService>(new WindowService(_services.Single<IUIFactory>()));
      _services.RegisterSingle<IGameFactory>(InitGameFactory());
      _services.RegisterSingle<IStateLoadService>(new StateLoadService(_services.Single<IPersistentProgressService>(), _services.Single<IGameFactory>()));
    }

    private void RegisterAssetProvider() {
      IAssetProvider assetProvider = new AssetProvider();
      assetProvider.Initialize();
      _services.RegisterSingle<IAssetProvider>(assetProvider);
    }

    private void RegisterAdsService() {
      IAdsService adsServices = new AdsService();
      adsServices.Initialize();
      _services.RegisterSingle<IAdsService>(adsServices);
    }
    
    private void RegisterIAPService(IAPProvider provider, IPersistentProgressService progress) {
      IIAPService iapService = new IAPService(provider, progress);
      iapService.Initialize();
      _services.RegisterSingle<IIAPService>(iapService);
    }

    private GameFactory InitGameFactory() {
      return new GameFactory(_services.Single<IAssetProvider>(), _services.Single<IStaticDataService>(), _services.Single<IRandomService>(),
        _services.Single<IPersistentProgressService>(), _services.Single<IWindowService>());
    }

    private void RegisterStaticDataService() {
      IStaticDataService staticDataService = new StaticDataService();
      staticDataService.LoadMonsters();
      _services.RegisterSingle(staticDataService);
    }
  }
}