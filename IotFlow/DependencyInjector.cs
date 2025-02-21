using Autofac;

namespace IotFlow
{
    public static class DependencyInjector
    {
        public static void Load(ContainerBuilder containerBuilder)
        {
            IotFlow.DataAccess.DependencyInjector.Load(containerBuilder);
            IotFlow.Utilities.DependencyInjector.Load(containerBuilder);
            IotFlow.Adapters.DependencyInjector.Load(containerBuilder);
            IotFlow.Services.DependencyInjector.Load(containerBuilder);
        }
    }
}
