using System.Linq;
using System.Web.Mvc;
using Unity.AspNet.Mvc;
using Project.Kernel;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Backend.Web.App_Start.UnityWebActivator), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Backend.Web.App_Start.UnityWebActivator), "Shutdown")]

namespace Backend.Web.App_Start
{
    public static class UnityWebActivator
    {
        public static void Start() 
        {
            var container = UnityConfig.GetConfiguredContainer();

            FilterProviders.Providers.Remove(FilterProviders.Providers.OfType<FilterAttributeFilterProvider>().First());
            FilterProviders.Providers.Add(new UnityFilterAttributeFilterProvider(container));

            DependencyResolver.SetResolver(new UnityDependencyResolver(container));

        }

        public static void Shutdown()
        {
            var container = UnityConfig.GetConfiguredContainer();
            container.Dispose();
        }
    }
}