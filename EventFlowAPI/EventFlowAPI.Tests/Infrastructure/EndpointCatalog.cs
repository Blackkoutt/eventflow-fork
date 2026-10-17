using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlowAPI.Tests.Infrastructure
{
    public record EndpointInfo(string Method, string Route, string Access)
    {
        public string Describe()
        {
            return $"{Method} /{Route} => {Access}";
        }
    }

    public class EndpointCatalog
    {
        public const string PublicAccess = "public";
        public const string AuthenticatedAccess = "authenticated";
        public const string AdminAccess = "admin";

        private static readonly Regex RouteParameter = new(@"\{[^}]+\}");

        private readonly EndpointDataSource _dataSource;

        public EndpointCatalog(IServiceProvider services)
        {
            _dataSource = services.GetRequiredService<EndpointDataSource>();
        }

        public List<EndpointInfo> GetAll()
        {
            var result = new List<EndpointInfo>();
            foreach (var endpoint in _dataSource.Endpoints.OfType<RouteEndpoint>())
            {
                var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                var methods = endpoint.Metadata.GetMetadata<IHttpMethodMetadata>();
                if (descriptor == null || methods == null)
                {
                    continue;
                }

                foreach (var method in methods.HttpMethods)
                {
                    result.Add(new EndpointInfo(method, endpoint.RoutePattern.RawText!, GetAccess(endpoint)));
                }
            }

            return result.OrderBy(endpoint => endpoint.Route, StringComparer.Ordinal)
                .ThenBy(endpoint => endpoint.Method, StringComparer.Ordinal)
                .ToList();
        }

        public string ToConcreteUrl(EndpointInfo endpoint)
        {
            return "/" + RouteParameter.Replace(endpoint.Route, "1");
        }

        private string GetAccess(RouteEndpoint endpoint)
        {
            if (endpoint.Metadata.GetMetadata<IAllowAnonymous>() != null)
            {
                return PublicAccess;
            }

            var rules = endpoint.Metadata.GetOrderedMetadata<IAuthorizeData>();
            if (rules.Count == 0)
            {
                return PublicAccess;
            }

            foreach (var rule in rules)
            {
                if (!string.IsNullOrEmpty(rule.Roles))
                {
                    return AdminAccess;
                }
            }

            return AuthenticatedAccess;
        }
    }
}
