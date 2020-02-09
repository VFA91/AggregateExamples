namespace Example_1.Domain.Repositories
{
    using Kernel.Library.Shared;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class GamesRepository : IRepository<Game>
    {
        public async Task Add(Game entity, CancellationToken cancellationToken)
        {
            await EnsureUniqueness(entity);
            await InternalAdd(entity, cancellationToken);
        }

        public async Task Remove(Game entity)
        {
            await EnsureIsNotInUse(entity);
            InternalRemove(entity);
        }

        public abstract Task<Game> Find(int id, CancellationToken cancellationToken);
        public abstract Task EnsureUniqueness(Game entity);
        public abstract Task EnsureIsNotInUse(Game entity);
        protected abstract Task InternalAdd(Game entity, CancellationToken cancellationToken);
        protected abstract void InternalRemove(Game entity);
    }
}
