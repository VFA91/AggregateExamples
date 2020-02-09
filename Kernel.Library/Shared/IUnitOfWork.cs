namespace Kernel.Library.Shared
{
    using System.Threading.Tasks;

    public interface IUnitOfWork
    {
        Task Save();
    }
}
