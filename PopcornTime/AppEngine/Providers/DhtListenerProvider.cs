using System.Net;
using Autofac;
using PopcornTime.Helpers;
using PopcornTime.Utilities.Interfaces;
using Universal.Torrent.Dht.Listeners;

namespace PopcornTime.AppEngine.Providers
{
    internal class DhtListenerProvider : IProvider<DhtListener>
    {
        public DhtListener CreateInstance(IComponentContext context)
        {
            var settingsUtility = context.Resolve<ISettingsUtility>();
            var port = settingsUtility.Read(ApplicationConstants.DhtPortKey, ApplicationConstants.DefaultDhtPort);
            return new DhtListener(new IPEndPoint(IPAddress.Any, port));
        }
    }
}