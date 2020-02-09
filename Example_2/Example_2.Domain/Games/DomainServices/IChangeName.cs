namespace Example_2.Domain.Games.DomainServices
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface IChangeName
    {
        Task Execute(int gameId, string name, CancellationToken cancellationToken);
    }
}
