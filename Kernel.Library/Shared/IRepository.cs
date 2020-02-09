namespace Kernel.Library.Shared
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IRepository<T>
         where T : AggregateRoot
    {
        Task Add(T entity, CancellationToken cancellationToken);
        Task<T> Find(int id, CancellationToken cancellationToken);
        Task Remove(T entity);
    }
}
