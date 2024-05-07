namespace ChessMaster.Contracts.Mappings;

public static class GameExtensions
{
    public static GameResponse ToGameResponse(this Game game)
    {
        return new GameResponse
        {
            Id = game.Id,
            Name = game.Name,
            Description = game.Description,
            Image = game.Image,
            Price = game.Price,
            ReleaseDate = game.ReleaseDate,
            GameCategory = game.GameCategory.ToGameCategoryResponse()
        };
    }

    public static GameCategoryResponse ToGameCategoryResponse(this GameCategory gameCategory)
    {
        return new GameCategoryResponse
        {
            Id = gameCategory.Id,
            Name = gameCategory.Name
        };
    }
}