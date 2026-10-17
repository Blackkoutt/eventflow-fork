using System.Reflection;
using EventFlowAPI.DB.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EventFlowAPI.Tests.Infrastructure
{
    public class DatabaseProbe
    {
        private static readonly MethodInfo SetMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!;

        private readonly IServiceProvider _services;

        public DatabaseProbe(IServiceProvider services)
        {
            _services = services;
        }

        public async Task<Dictionary<string, int>> CountRowsAsync()
        {
            using var scope = _services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<APIContext>();
            var result = new Dictionary<string, int>();
            foreach (var entityType in context.Model.GetEntityTypes())
            {
                if (entityType.HasSharedClrType || entityType.IsOwned() || entityType.FindPrimaryKey() == null)
                {
                    continue;
                }

                var set = (IQueryable<object>)SetMethod.MakeGenericMethod(entityType.ClrType).Invoke(context, null)!;
                result[entityType.ClrType.Name] = await set.CountAsync();
            }

            return result;
        }

        public async Task<List<string>> ChangesDuringAsync(Func<Task> action)
        {
            var before = await CountRowsAsync();
            await action();
            var after = await CountRowsAsync();
            var changes = new List<string>();
            foreach (var table in after.Keys.OrderBy(name => name, StringComparer.Ordinal))
            {
                var difference = after[table] - before.GetValueOrDefault(table);
                if (difference != 0)
                {
                    changes.Add($"{table} {difference:+0;-0}");
                }
            }

            return changes;
        }
    }
}
