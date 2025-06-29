using FinanceManager.Application.Accounts.Dtos;
using FinanceManager.Application.Accounts.Interfaces;
using FinanceManager.Application.Accounts.Mapping;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Models.Paging;

namespace FinanceManager.Application.Accounts.Services;

public class AccountService(
    IAccountRepository accountRepository,
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser,
    AccountMapper mapper) : IAccountService
{
    public async Task<AccountResponseDto?> GetByIdAsync(int id, CancellationToken ct)
        => await accountRepository.GetByIdAsync(id, currentUser.UserId, ct);

    public async Task<PagedResult<AccountResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct)
        => await accountRepository.GetAllAsync(currentUser.UserId, paging, ct);

    public async Task<int> CreateAsync(CreateAccountDto dto, CancellationToken ct)
    {
        var entity = mapper.ToEntity(dto);
        await accountRepository.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(UpdateAccountDto dto, CancellationToken ct)
    {
        var account = await accountRepository.FindEntityByIdAsync(dto.Id, currentUser.UserId, ct);
        if (account is null)
            return false;

        mapper.UpdateEntity(dto, account);

        await accountRepository.UpdateAsync(account, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var account = await accountRepository.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (account is null)
            return false;

        await accountRepository.DeleteAsync(account, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}
