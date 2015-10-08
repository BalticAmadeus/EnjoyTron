using Autofac;
using Tron.DebugClient.Demo.Infrastructure;
using Tron.DebugClient.Demo.ViewModel;
using Tron.DebugClient.Demo.ViewModel.Flows;
using Tron.DebugClient.Demo.Views;
using Tron.DebugClient.Demo.Views.Flows;

namespace Tron.DebugClient.Demo
{

    public class Module: Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterType<SettingsManager>().As<ISettingsManager>().SingleInstance();
            builder.RegisterType<ServiceCallInvoker>().As<IServiceCallInvoker>().SingleInstance();
            builder.RegisterType<CommonDataManager>().As<ICommonDataManager>().SingleInstance();
            builder.RegisterType<MapService>().As<IMapService>().SingleInstance();
            builder.RegisterType<MessageBoxDialogService>().As<IMessageBoxDialogService>().InstancePerDependency();

            builder.RegisterType<LoggerViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<CommonDataViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<CompleteLoginViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<CreatePlayerViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<InitLoginViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<LeaveGameViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<WaitGameStartViewModel>().AsSelf().SingleInstance();
            builder.RegisterType<CompleteLoginFlowViewModel>().AsSelf().SingleInstance();

            builder.RegisterType<MainView>().Named<object>("MainView").SingleInstance();
            builder.RegisterType<EmptyView>().Named<object>("EmptyView").SingleInstance();
            builder.RegisterType<CommonDataView>().Named<object>("CommonDataView").SingleInstance();
            builder.RegisterType<LoggerView>().Named<object>("LoggerView").SingleInstance();
            builder.RegisterType<CompleteLoginView>().Named<object>("CompleteLoginView").SingleInstance();
            builder.RegisterType<CreatePlayerView>().Named<object>("CreatePlayerView").SingleInstance();
            builder.RegisterType<InitLoginView>().Named<object>("InitLoginView").SingleInstance();
            builder.RegisterType<LeaveGameView>().Named<object>("LeaveGameView").SingleInstance();
            builder.RegisterType<WaitGameStartView>().Named<object>("WaitGameStartView").SingleInstance();
            builder.RegisterType<CompleteLoginFlowView>().Named<object>("CompleteLoginFlowView").SingleInstance();
        }
    }
}
