using System.Collections.Generic;
using System.Windows;
using Microsoft.Practices.Prism.Logging;
using Microsoft.Practices.Prism.Modularity;
using Microsoft.Practices.Prism.UnityExtensions;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Tron.DebugClient.Demo.Utilites;

namespace Tron.DebugClient.Demo
{
    public class Bootstrapper : UnityBootstrapper
    {
        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<ShellWindow>();
        }

        protected override void InitializeShell()
        {
            base.InitializeShell();

            Application.Current.MainWindow = (Window)Shell;
            Application.Current.MainWindow.Show();
        }

        protected override ILoggerFacade CreateLogger()
        {
            return new CompositeLogger(new List<ILoggerFacade>
            {
                //new TextLogger(),
                new FileLogger(),
            });
        }

        protected override void ConfigureServiceLocator()
        {
            ServiceLocator.SetLocatorProvider(() => Container.Resolve<IServiceLocator>());
        }

        protected override void ConfigureModuleCatalog()
        {
            base.ConfigureModuleCatalog();

            var mainModule = typeof(Module);
            ModuleCatalog.AddModule(new ModuleInfo(mainModule.Name, mainModule.AssemblyQualifiedName) { InitializationMode = InitializationMode.WhenAvailable });
        }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
