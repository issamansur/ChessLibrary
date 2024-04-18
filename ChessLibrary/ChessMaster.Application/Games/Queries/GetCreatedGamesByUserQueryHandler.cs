using ChessMaster.Domain.States;

namespace ChessMaster.Application.Games.Queries;

public class GetCreatedGamesByUserQueryHandler : BaseHandler,
    IRequestHandler<GetCreatedGamesByUserQuery, IReadOnlyCollection<Game>>
{
    public GetCreatedGamesByUserQueryHandler(ITenantFactory tenantFactory) :
        base(tenantFactory)
    {
    }

    public async Task<IReadOnlyCollection<Game>> Handle(GetCreatedGamesByUserQuery request,
        CancellationToken cancellationToken)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        var games = await GetTenant().Games.GetByUser(request.UserId, cancellationToken);
        var createdGames = (IReadOnlyCollection<Game>)games.Where(game => game.State == State.Created);

        return createdGames;
    }
}