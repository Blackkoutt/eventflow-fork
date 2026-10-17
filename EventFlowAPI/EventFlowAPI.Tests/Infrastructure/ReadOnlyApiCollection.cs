namespace EventFlowAPI.Tests.Infrastructure
{
    [CollectionDefinition(Name)]
    public class ReadOnlyApiCollection : ICollectionFixture<ApiFactory>
    {
        public const string Name = "ReadOnlyApi";
    }
}
