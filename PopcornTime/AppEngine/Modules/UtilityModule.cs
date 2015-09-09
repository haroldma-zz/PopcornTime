using Autofac;
using PopcornTime.AppEngine.Providers;
using PopcornTime.Utilities.DesignTime;
using PopcornTime.Utilities.Interfaces;
using PopcornTime.Utilities.RunTime;
using Universal.Torrent.Client;
using Universal.Torrent.Dht;
using Universal.Torrent.Dht.Listeners;

namespace PopcornTime.AppEngine.Modules
{
    internal class UtilityModule : AppModule
    {
        public override void LoadDesignTime(ContainerBuilder builder)
        {
            builder.RegisterType<DesignDispatcherUtility>().As<IDispatcherUtility>();
            builder.RegisterType<DesignCredentialUtility>().As<ICredentialUtility>();
            builder.RegisterType<DesignSettingsUtility>().As<ISettingsUtility>();
        }

        public override void LoadRunTime(ContainerBuilder builder)
        {
            builder.RegisterType<DispatcherUtility>().As<IDispatcherUtility>();
            builder.RegisterType<CredentialUtility>().As<ICredentialUtility>();
            builder.RegisterType<SettingsUtility>().As<ISettingsUtility>();

            builder.RegisterType<ClientEngine, ClientEngineProvider>().SingleInstance();
            builder.RegisterType<DhtListener, DhtListenerProvider>().SingleInstance();
            builder.RegisterType<DhtEngine>().As<IDhtEngine>().SingleInstance();
        }
    }
}