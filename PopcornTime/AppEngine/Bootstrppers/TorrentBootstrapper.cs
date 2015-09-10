using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using PopcornTime.Helpers;
using PopcornTime.Utilities.Interfaces;
using Universal.Nat;
using Universal.Nat.Enums;
using Universal.Torrent.Client;
using Universal.Torrent.Common;
using Universal.Torrent.Dht.Listeners;

namespace PopcornTime.AppEngine.Bootstrppers
{
    public class TorrentBootstrapper : AppBootStrapper
    {
        public override void OnLaunched(IComponentContext context)
        {
            Start(context);
        }

        public override void OnRelaunched(IComponentContext context, Dictionary<string, object> state)
        {
            Start(context);
        }

        internal void Start(IComponentContext context)
        {
            var engine = context.Resolve<ClientEngine>();
            var dhtListner = context.Resolve<DhtListener>();
            var dht = context.Resolve<IDhtEngine>();
            var settingsUtility = context.Resolve<ISettingsUtility>();

            var port = settingsUtility.Read(ApplicationConstants.TorrentPortKey,
                   ApplicationConstants.DefaultTorrentPort);

          OpenPort(port);

            // register the dht engine
            engine.RegisterDht(dht);

            // start the dht listener
            dhtListner.Start();

            // annnnnddd start the dht engine
            engine.DhtEngine.Start();
            
            // clear up torrent folder
            Task.Factory.StartNew(async () =>
            {
                var torrentsFolder = engine.Settings.SaveFolder;
                await StorageHelper.DeleteFolderContentAsync(torrentsFolder);
            });
        }

        private async void OpenPort(int port)
        {
            try
            {
                // port mapping
                var discoverer = new NatDiscoverer();
                NatDevice device;
                using (var cts = new CancellationTokenSource())
                    device = await discoverer.DiscoverDeviceAsync(PortMapper.Upnp, cts);
                await device.CreatePortMapAsync(new Mapping(Protocol.Tcp, port, port, 0, "Popcorn Time"));
                Debug.WriteLine("Port opened");
            }
            catch
            {
                // ignored
                Debug.WriteLine("Port couldn't be open.");
            }
        }
    }
}