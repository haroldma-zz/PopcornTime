using Autofac;
using PopcornTime.Common;
using PopcornTime.Helpers;
using PopcornTime.Utilities.Interfaces;
using Universal.Torrent.Client;
using Universal.Torrent.Client.Encryption;
using Universal.Torrent.Client.Settings;
using Universal.Torrent.Common;

namespace PopcornTime.AppEngine.Providers
{
    internal class ClientEngineProvider : IProvider<ClientEngine>
    {
        public ClientEngine CreateInstance(IComponentContext context)
        {
            var settingsUtility = context.Resolve<ISettingsUtility>();
            var port = settingsUtility.Read(ApplicationConstants.TorrentPortKey, ApplicationConstants.DefaultTorrentPort);

            var engineSettings =
                new EngineSettings(AsyncHelper.RunSync(() => StorageHelper.EnsureFolderExistsAsync("torrents", StorageHelper.StorageStrategy.Temporary)) , port)
                {
                    PreferEncryption = true,
                    AllowedEncryption = EncryptionTypes.All
                };
            // Create an instance of the engine.
            return new ClientEngine(engineSettings);
        }
    }
}