using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.Regions;
using Microsoft.Practices.Unity;
using Tron.AdminClient.Infrastructure;
using Tron.AdminClient.ViewModels;
using Tron.AdminClient.Views;

namespace Tron.AdminClient
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
            _container
                .RegisterType<ISettingsManager, SettingsManager>(new ContainerControlledLifetimeManager())
                .RegisterType<IFileDialogService, FileDialogService>()
                .RegisterType<IConfirmationDialogService, ConfirmationDialogService>()
                .RegisterType<IMapService, MapService>()
                .RegisterType<IMessageBoxDialogService, MessageBoxDialogService>()
                .RegisterType<IAdministrationServiceGateway, AdministrationServiceGateway>(new ContainerControlledLifetimeManager())
            
                .RegisterType<LoginViewModel>()
                .RegisterType<LobbyViewModel>()
                .RegisterType<OpenGameViewModel>()
                .RegisterType<GamePreviewViewModel>()

                .RegisterType<object, LoginView>("LoginView")
                .RegisterType<object, LobbyView>("LobbyView")
                .RegisterType<object, OpenGameView>("OpenGameView")
                .RegisterType<object, GamePreviewView>("GamePreviewView");

            _regionManager.RegisterViewWithRegion("MainRegion", () => _container.Resolve<LoginView>("LoginView"));
        }
    }
}
