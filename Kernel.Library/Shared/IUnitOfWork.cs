namespace Kernel.Library.Shared
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task Save(CancellationToken cancellationToken);
    }
}
