using MediatR;

namespace Application.Users.Commands.RegisterUser
{
    public record RegisterUserCommand(
        string Username,
        string Email,
        string FullName,
        string Password,
        string ConfirmPassword,
        int Age,
        string PhoneNumber
    ) : IRequest<RegisterUserResponse>;

    public record RegisterUserResponse(
        string Token
    );
}
