using FinanceManager.Application.Transactions.Dtos;
using FinanceManager.Application.Transactions.Interfaces;
using FinanceManager.Application.Transactions.Mapping;
using FinanceManager.Application.Common.Interfaces;
using FinanceManager.Application.Common.Interfaces.Persistence;
using FinanceManager.Application.Common.Interfaces.Paging;
using FinanceManager.Application.Common.Models.Paging;
using FinanceManager.Application.Common.Exceptions;

namespace FinanceManager.Application.Transactions.Services;

public class TransactionService(
    IUnitOfWork unitOfWork,
    ICurrentUserService currentUser,
    IPagingService pagingService,
    TransactionMapper mapper) : ITransactionService
{
    public async Task<TransactionResponseDto?> GetByIdAsync(int id, CancellationToken ct)
    {
        var account = await unitOfWork.Transactions.GetByIdAsync(id, currentUser.UserId, ct);
        return account is null ? null : mapper.ToDto(account);
    }

    public async Task<PagedResult<TransactionResponseDto>> GetAllAsync(PagedRequest paging, CancellationToken ct)
    {
        var query = unitOfWork.Transactions.GetAllAsync(currentUser.UserId);
        return await pagingService.ToPagedResultAsync(query, paging, mapper.ToDto, ct);
    }

    public async Task<int> CreateAsync(CreateTransactionDto dto, CancellationToken ct)
    {
        var sourceAccount =
            await unitOfWork.Accounts.GetByIdAsync(dto.SourceAccountId, currentUser.UserId, ct);
        if (sourceAccount == null)
        {
            throw new NotFoundException($"Source account with ID {dto.SourceAccountId} not found.");
        }

        var targetAccount =
            await unitOfWork.Accounts.GetByIdAsync(dto.DestinationAccountId, currentUser.UserId, ct);
        if (targetAccount == null)
        {
            throw new NotFoundException($"Target account with ID {dto.DestinationAccountId} not found.");
        }

        if (dto.CategoryId.HasValue)
        {
            var category =
                await unitOfWork.Categories.GetByIdAsync(dto.CategoryId.Value, currentUser.UserId, ct);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {dto.CategoryId.Value} not found.");
            }
        }

        var entity = mapper.ToEntity(dto);
        await unitOfWork.Transactions.AddAsync(entity, ct);
        await unitOfWork.SaveChangesAsync(ct);

        return entity.Id;
    }

    public async Task<bool> UpdateAsync(int id, UpdateTransactionDto dto, CancellationToken ct)
    {
        var account = await unitOfWork.Transactions.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (account is null)
            return false;

        var sourceAccount =
            await unitOfWork.Accounts.GetByIdAsync(dto.SourceAccountId, currentUser.UserId, ct);
        if (sourceAccount == null)
        {
            throw new NotFoundException($"Source account with ID {dto.SourceAccountId} not found.");
        }

        var targetAccount =
            await unitOfWork.Accounts.GetByIdAsync(dto.DestinationAccountId, currentUser.UserId, ct);
        if (targetAccount == null)
        {
            throw new NotFoundException($"Target account with ID {dto.DestinationAccountId} not found.");
        }

        if (dto.CategoryId.HasValue)
        {
            var category =
                await unitOfWork.Categories.GetByIdAsync(dto.CategoryId.Value, currentUser.UserId, ct);
            if (category == null)
            {
                throw new NotFoundException($"Category with ID {dto.CategoryId.Value} not found.");
            }
        }

        mapper.UpdateEntity(dto, account);

        await unitOfWork.Transactions.UpdateAsync(account, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken ct)
    {
        var account = await unitOfWork.Transactions.FindEntityByIdAsync(id, currentUser.UserId, ct);
        if (account is null)
            return false;

        await unitOfWork.Transactions.DeleteAsync(account, ct);
        await unitOfWork.SaveChangesAsync(ct);
        return true;
    }
}