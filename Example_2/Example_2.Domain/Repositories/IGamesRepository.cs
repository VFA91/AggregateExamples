﻿namespace Example_2.Domain.Repositories
{
    using Example_2.Domain.Games;
    using Kernel.Library.Shared;
    using System;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IGamesRepository : IRepository<Game>
    {
        Task<bool> AnyAsync(Expression<Func<Game, bool>> specification, CancellationToken cancellationToken);
    }
}
