using System.Reflection;
using Autofac;
using PopcornTime.Extensions;
using PopcornTime.Tools.Mvvm;
using Module = Autofac.Module;

namespace PopcornTime.AppEngine.Modules
{
    internal class ViewModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // Every view model is a child of this base
            var viewModelBase = typeof (ViewModelBase);

            // they should also be located in that assembly
            var assembly = viewModelBase.GetTypeInfo().Assembly;

            var types = assembly.DefinedTypes.GetImplementations(viewModelBase);
            foreach (var type in types)
            {
                builder.RegisterType(type);
            }
        }
    }
}