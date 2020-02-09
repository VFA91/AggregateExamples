namespace Example_2.Domain.Repositories
{
    using Kernel.Library.Shared;
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IVenuesRepository : IRepository<Venue>
    {
        Task<bool> AnyAsync(Expression<Func<Venue, bool>> specification, CancellationToken cancellationToken);
    }
}
