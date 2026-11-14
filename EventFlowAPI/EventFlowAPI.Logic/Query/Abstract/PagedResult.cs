namespace EventFlowAPI.Logic.Query.Abstract
{
    /// <summary>
    /// One page of records returned by a repository. The database does the filtering, sorting and paging,
    /// so the service never loads the whole collection.
    /// </summary>
    public sealed record PagedResult<T>(IReadOnlyList<T> Items, int TotalCount, int? PageNumber, int? PageSize)
    {
        public static PagedResult<T> Empty(int? pageNumber = null, int? pageSize = null)
            => new([], 0, pageNumber, pageSize);
    }
}
