using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.Accounts.Dtos;

public class UpdateAccountDto
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public AccountType Type { get; set; }

    public string? Iban { get; set; }

    public bool IncludedInNetWorth { get; set; }

    public bool CanTransferFrom { get; set; }

    public bool CanTransferTo { get; set; }
}
