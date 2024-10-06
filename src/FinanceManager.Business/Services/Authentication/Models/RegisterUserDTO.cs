namespace FinanceManager.Business.Services.Models;

public class RegisterUserDTO
{
    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public string? EmailAddress { get; init; }

    public string? Password { get; init; }
}