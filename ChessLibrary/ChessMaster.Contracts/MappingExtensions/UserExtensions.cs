using ChessMaster.Application.CQRS.Users.Queries;
using ChessMaster.Contracts.DTOs.Users;

namespace ChessMaster.Contracts.MappingExtensions;

public static class UserExtensions
{
    public static GetUserQuery ToQuery(this GetUserRequest request)
    {
        return new GetUserQuery(request.Id);
    }

    public static GetUserResponse ToGetResponse(this User user)
    {
        return new GetUserResponse(
            user.Id,
            user.Username
        );
    }
    
    public static GetUserByUsernameQuery ToQuery(this GetUserByUsernameRequest request)
    {
        return new GetUserByUsernameQuery(request.Username);
    }
    
    public static GetUserResponse ToGetByUsernameResponse(this User user)
    {
        return new GetUserResponse(
            user.Id,
            user.Username
        );
    }
    
    public static SearchUserQuery ToQuery(this SearchUserRequest request)
    {
        return new SearchUserQuery(request.Query);
    }
    
    public static SearchUserResponse ToSearchResponse(this IReadOnlyCollection<User> users)
    {
        return new SearchUserResponse(
            users.Select(
                user => new GetUserResponse(
                    user.Id,
                    user.Username
                )
            ).ToList()
        );
    }
}