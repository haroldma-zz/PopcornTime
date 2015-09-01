using Autofac;
using PopcornTime.Helpers;
using PopcornTime.Utilities.Interfaces;
using Universal.Nat;

namespace PopcornTime.AppEngine.Providers
{
    internal class NatManagerProvider : IProvider<NatManager>
    {
        public NatManager CreateInstance(IComponentContext context)
        {
            var settingsUtility = context.Resolve<ISettingsUtility>();
            var port = settingsUtility.Read(ApplicationConstants.TorrentPortKey, ApplicationConstants.DefaultTorrentPort);
            return new NatManager(port);
        }
    }
}