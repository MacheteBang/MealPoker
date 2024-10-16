namespace MealBot.Api.Auth.Features.GetUser;

internal sealed class GetUserHandler(IUserService userService) : IRequestHandler<GetUserQuery, ErrorOr<User>>
{
    private readonly IUserService _userService = userService;

    public Task<ErrorOr<User>> Handle(GetUserQuery query, CancellationToken cancellationToken)
    {
        return _userService.GetByUserIdAsync(query.UserId);
    }
}