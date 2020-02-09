namespace Example_2.Domain.Games.DomainServices
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IDeleteGame
    {
        Task Execute(int gameId, CancellationToken cancellationToken);
    }
}
