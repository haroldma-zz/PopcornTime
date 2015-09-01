using Autofac;

namespace PopcornTime.AppEngine.Providers
{
    public interface IProvider<out T>
    {
        T CreateInstance(IComponentContext context);
    }
}