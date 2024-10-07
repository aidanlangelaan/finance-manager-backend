namespace FinanceManager.Business.Services.Models;

public class RegisterUserDTO
{
    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public string EmailAddress { get; init; } = null!;
}