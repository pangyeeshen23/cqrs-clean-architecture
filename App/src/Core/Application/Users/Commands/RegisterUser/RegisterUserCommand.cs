using MediatR;

namespace Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(
        string Email,
        string Name,
        string Password,
        string ConfirmPassword,
        int Age,
        string PhoneNumber,
        string Address
    ) : IRequest<RegisterUserResponse>;

    public record RegisterUserResponse(
        Guid UserId,
        string Email,
        string Name,
        string Token
    );
}
