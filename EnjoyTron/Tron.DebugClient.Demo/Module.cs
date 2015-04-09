using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Tron.DebugClient.Demo.Infrastructure;
using Tron.DebugClient.Demo.ViewModel;
using Tron.DebugClient.Demo.ViewModel.Flows;
using Tron.DebugClient.Demo.Views;
using Tron.DebugClient.Demo.Views.Flows;

namespace Tron.DebugClient.Demo
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
            _container.RegisterType<IMessageBoxDialogService, MessageBoxDialogService>();

            _container.RegisterType<LoggerViewModel>(new TransientLifetimeManager());
            _container.RegisterType<CommonDataViewModel>(new TransientLifetimeManager());

            _container.RegisterType<CompleteLoginViewModel>(new TransientLifetimeManager());
            _container.RegisterType<CreatePlayerViewModel>(new TransientLifetimeManager());
            _container.RegisterType<InitLoginViewModel>(new TransientLifetimeManager());
            _container.RegisterType<LeaveGameViewModel>(new TransientLifetimeManager());
            _container.RegisterType<WaitGameStartViewModel>(new TransientLifetimeManager());
            _container.RegisterType<CompleteLoginFlowViewModel>(new TransientLifetimeManager());

            _container.RegisterType<object, MainView>("MainView");
            _container.RegisterType<object, EmptyView>("EmptyView");
            _container.RegisterType<object, CommonDataView>("CommonDataView");
            _container.RegisterType<object, LoggerView>("LoggerView");

            _container.RegisterType<object, CompleteLoginView>("CompleteLoginView", new TransientLifetimeManager());
            _container.RegisterType<object, CreatePlayerView>("CreatePlayerView", new TransientLifetimeManager());
            _container.RegisterType<object, InitLoginView>("InitLoginView", new TransientLifetimeManager());
            _container.RegisterType<object, LeaveGameView>("LeaveGameView", new TransientLifetimeManager());
            _container.RegisterType<object, WaitGameStartView>("WaitGameStartView", new TransientLifetimeManager());
            _container.RegisterType<object, CompleteLoginFlowView>("CompleteLoginFlowView", new TransientLifetimeManager());

            _regionManager.RegisterViewWithRegion("MainRegion", () => _container.Resolve<MainView>("MainView"));
            _regionManager.RegisterViewWithRegion("AuthRegion", () => _container.Resolve<CommonDataView>("CommonDataView"));
            _regionManager.RegisterViewWithRegion("LoggerRegion", () => _container.Resolve<LoggerView>("LoggerView"));

            _regionManager.RegisterViewWithRegion("ContentRegion", () => _container.Resolve<EmptyView>("EmptyView"));
        }
    }
}
