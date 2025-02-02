using Autofac;

namespace IotFlow.Utilities
{
    public static class DependencyInjector
    {
        public static void Load(ContainerBuilder containerBuilder)
        {
            containerBuilder
                .RegisterAssemblyTypes(typeof(DependencyInjector).Assembly)
                .AsSelf()
                .AsImplementedInterfaces();
        }
    }
}