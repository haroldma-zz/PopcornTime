using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Autofac;
using Universal.Nat;
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
            var natManager = context.Resolve<NatManager>();
            var dhtListner = context.Resolve<DhtListener>();
            var dht = context.Resolve<IDhtEngine>();

            // start the nat manager
            natManager.Start();

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
    }
}