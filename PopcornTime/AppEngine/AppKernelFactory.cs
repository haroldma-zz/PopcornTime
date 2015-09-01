using Autofac;
using PopcornTime.AppEngine.Bootstrppers;
using PopcornTime.AppEngine.Modules;

namespace PopcornTime.AppEngine
{
    internal static class AppKernelFactory
    {
        public static Module[] GetModules() =>
            new Module[]
            {
                new UtilityModule(),
                new ServiceModule(),
                new ViewModelModule()
            };

        public static IBootStrapper[] GetBootStrappers() =>
            new IBootStrapper[]
            {
                new TorrentBootstrapper()
            };

        public static AppKernel Create() => new AppKernel(GetBootStrappers(), GetModules());
    }
}