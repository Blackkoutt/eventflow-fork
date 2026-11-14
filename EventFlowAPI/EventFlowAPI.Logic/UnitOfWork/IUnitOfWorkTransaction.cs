namespace EventFlowAPI.Logic.UnitOfWork
{
    /// <summary>
    /// A database transaction that does not depend on the database engine.
    /// Disposing it without calling <see cref="CommitAsync"/> rolls the changes back.
    /// </summary>
    public interface IUnitOfWorkTransaction : IAsyncDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);
        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
