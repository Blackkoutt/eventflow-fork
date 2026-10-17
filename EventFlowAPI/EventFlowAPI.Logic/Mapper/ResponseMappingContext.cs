namespace EventFlowAPI.Logic.Mapper;

// Each response graph owns its cache, so cyclic navigation properties terminate
// without sharing state between requests or application hosts.
internal sealed class ResponseMappingContext
{
    private readonly Dictionary<object, object> _responses = new(ReferenceEqualityComparer.Instance);

    public bool TryGet<T>(object source, out T response) where T : class
    {
        if (_responses.TryGetValue(source, out var existing))
        {
            response = (T)existing;
            return true;
        }
        response = null!;
        return false;
    }

    public void Add(object source, object response) => _responses.Add(source, response);
}
