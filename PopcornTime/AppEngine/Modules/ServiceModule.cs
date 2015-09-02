using Audiotica.Windows.Services.NavigationService;
using Autofac;
using PopcornTime.Services.DesignTime;
using PopcornTime.Services.Interfaces;
using PopcornTime.Services.NavigationService;
using PopcornTime.Services.RunTime;

namespace PopcornTime.AppEngine.Modules
{
    internal class ServiceModule : AppModule
    {
        public override void LoadDesignTime(ContainerBuilder builder)
        {
            builder.RegisterType<DesignInsightsService>().As<IInsightsService>();
            builder.RegisterType<DesignNavigationService>().As<INavigationService>();
        }

        public override void LoadRunTime(ContainerBuilder builder)
        {
            builder.RegisterType<InsightsService>().As<IInsightsService>();
            builder.RegisterType<NavigationService>().As<INavigationService>().SingleInstance();
            builder.RegisterType<TorrentStreamService>().As<ITorrentStreamService>().SingleInstance();
        }
    }
}