namespace FinanceManager.Api.Models;

public class RegisterUserViewModel
{
    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public string EmailAddress { get; init; } = null!;
}