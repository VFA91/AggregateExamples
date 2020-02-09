namespace Example_2.Domain.Games.DomainServices
{
    using System.Threading;
    using System.Threading.Tasks;

    public interface ICreateNewGame
    {
        Task<Game> Execute(string name, CancellationToken cancellationToken);
    }
}
