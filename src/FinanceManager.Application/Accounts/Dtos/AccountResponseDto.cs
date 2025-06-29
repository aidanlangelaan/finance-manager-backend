using FinanceManager.Domain.Enums;

namespace FinanceManager.Application.Accounts.Dtos;

public class AccountResponseDto
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public string? Description { get; set; }

    public AccountType Type { get; set; }

    public decimal CurrentBalance { get; set; }

    public bool IncludedInNetWorth { get; set; }

    public bool CanTransferFrom { get; set; }

    public bool CanTransferTo { get; set; }
}
