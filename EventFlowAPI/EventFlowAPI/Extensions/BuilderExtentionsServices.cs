using EventFlowAPI.Logic;
using EventFlowAPI.Logic.UnitOfWork;

namespace EventFlowAPI.Extensions
{
    public static class BuilderExtentionsServices
    {
        public static void AddUnitOfWork(this IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();
        }

        public static void AddApplicationServices(this IServiceCollection services)
        {
            var implementations = AssemblyReference.Assembly.GetTypes()
                .Where(type => type.IsClass
                            && !type.IsAbstract
                            && !type.IsGenericTypeDefinition
                            && type.Namespace != null
                            && type.Namespace.Split('.').Contains("Services"));

            foreach (var implementation in implementations)
            {
                var serviceInterface = implementation.GetInterfaces()
                    .FirstOrDefault(@interface => @interface.Name == $"I{implementation.Name}");

                if (serviceInterface != null && services.All(service => service.ServiceType != serviceInterface))
                {
                    services.AddScoped(serviceInterface, implementation);
                }
            }
        }
    }
}
