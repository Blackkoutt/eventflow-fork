using EventFlowAPI.Logic.Services.OtherServices.Interfaces;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Extensions
{
    public static class BuilderExtentionsServices
    {
        private static readonly string[] ServiceNamespaces =
        [
            "EventFlowAPI.Logic.Services",
            "EventFlowAPI.Logic.Identity.Services"
        ];

        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            var implementations = typeof(IBlobService).Assembly.GetTypes()
                .Where(type => type.IsClass
                            && !type.IsAbstract
                            && !type.IsGenericTypeDefinition
                            && type.Namespace != null
                            && ServiceNamespaces.Any(serviceNamespace => type.Namespace.StartsWith(serviceNamespace)));

            foreach (var implementation in implementations)
            {
                var serviceInterface = implementation.GetInterfaces()
                    .FirstOrDefault(@interface => @interface.Name == $"I{implementation.Name}");

                if (serviceInterface != null)
                {
                    services.AddScoped(serviceInterface, implementation);
                }
            }
        }
    }
}
