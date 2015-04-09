using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Tron.DebugClient.Infrastructure;
using Tron.DebugClient.ViewModel;
using Tron.DebugClient.ViewModel.Flows;
using Tron.DebugClient.Views;
using Tron.DebugClient.Views.Flows;

namespace Tron.DebugClient
{
    public class Module: IModule
    {
        private readonly IUnityContainer _container;
        private readonly IRegionManager _regionManager;

        public Module(IUnityContainer container, IRegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ISettingsManager, SettingsManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IServiceCallInvoker, ServiceCallInvoker>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICommonDataManager, CommonDataManager>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMapService, MapService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IMessageBoxDialogService, MessageBoxDialogService>(new TransientLifetimeManager());

            _container.RegisterType<LoggerViewModel>();
            _container.RegisterType<CommonDataViewModel>();

            _container.RegisterType<CompleteLoginViewModel>(new TransientLifetimeManager());
            _container.RegisterType<CreatePlayerViewModel>(new TransientLifetimeManager());
            _container.RegisterType<GetPlayerViewViewModel>(new TransientLifetimeManager());
            _container.RegisterType<GetTurnResultViewModel>(new TransientLifetimeManager());
            _container.RegisterType<InitLoginViewModel>(new TransientLifetimeManager());
            _container.RegisterType<LeaveGameViewModel>(new TransientLifetimeManager());
            _container.RegisterType<PerformMoveViewModel>(new TransientLifetimeManager());
            _container.RegisterType<WaitGameStartViewModel>(new TransientLifetimeManager());
            _container.RegisterType<WaitNextTurnViewModel>(new TransientLifetimeManager());

            _container.RegisterType<CompleteLoginFlowViewModel>(new TransientLifetimeManager());
            _container.RegisterType<CreatePlayerFlowViewModel>(new TransientLifetimeManager());
            _container.RegisterType<WaitGameStartFlowViewModel>(new TransientLifetimeManager());
            _container.RegisterType<PlayerModeFlowViewModel>(new TransientLifetimeManager());
            _container.RegisterType<WaitNextTurnLoopViewModel>(new TransientLifetimeManager());

            _container.RegisterType<object, MainView>("MainView");
            _container.RegisterType<object, EmptyView>("EmptyView");
            _container.RegisterType<object, CommonDataView>("CommonDataView");
            _container.RegisterType<object, LoggerView>("LoggerView");

            _container.RegisterType<object, CompleteLoginView>("CompleteLoginView", new TransientLifetimeManager());
            _container.RegisterType<object, CreatePlayerView>("CreatePlayerView", new TransientLifetimeManager());
            _container.RegisterType<object, GetPlayerViewView>("GetPlayerViewView", new TransientLifetimeManager());
            _container.RegisterType<object, GetTurnResultView>("GetTurnResultView", new TransientLifetimeManager());
            _container.RegisterType<object, InitLoginView>("InitLoginView", new TransientLifetimeManager());
            _container.RegisterType<object, LeaveGameView>("LeaveGameView", new TransientLifetimeManager());
            _container.RegisterType<object, PerformMoveView>("PerformMoveView", new TransientLifetimeManager());
            _container.RegisterType<object, WaitGameStartView>("WaitGameStartView", new TransientLifetimeManager());
            _container.RegisterType<object, WaitNextTurnView>("WaitNextTurnView", new TransientLifetimeManager());

            _container.RegisterType<object, CompleteLoginFlowView>("CompleteLoginFlowView", new TransientLifetimeManager());
            _container.RegisterType<object, CreatePlayerFlowView>("CreatePlayerFlowView", new TransientLifetimeManager());
            _container.RegisterType<object, WaitGameStartFlowView>("WaitGameStartFlowView", new TransientLifetimeManager());
            _container.RegisterType<object, PlayerModeFlowView>("PlayerModeFlowView", new TransientLifetimeManager());
            _container.RegisterType<object, WaitNextTurnLoopView>("WaitNextTurnLoopView", new TransientLifetimeManager());

            _regionManager.RegisterViewWithRegion("MainRegion", () => _container.Resolve<MainView>("MainView"));
            _regionManager.RegisterViewWithRegion("AuthRegion", () => _container.Resolve<CommonDataView>("CommonDataView"));
            _regionManager.RegisterViewWithRegion("LoggerRegion", () => _container.Resolve<LoggerView>("LoggerView"));

            _regionManager.RegisterViewWithRegion("ContentRegion", () => _container.Resolve<EmptyView>("EmptyView"));
        }
    }
}
