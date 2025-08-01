using FinanceManager.Application.ImportJobs.Dtos;
using FinanceManager.Domain.Entities;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Application.ImportJobs.Mapping;

[Mapper]
public partial class ImportJobMapper
{
    [MapperIgnoreTarget(nameof(ImportJob.Id))]
    [MapperIgnoreTarget(nameof(ImportJob.RowVersion))]
    [MapperIgnoreTarget(nameof(ImportJob.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(ImportJob.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(ImportJob.CreatedById))]
    [MapperIgnoreTarget(nameof(ImportJob.UpdatedById))]
    [MapperIgnoreTarget(nameof(ImportJob.CreatedBy))]
    [MapperIgnoreTarget(nameof(ImportJob.UpdatedBy))]
    [MapperIgnoreTarget(nameof(ImportJob.StoredFileName))]
    [MapperIgnoreTarget(nameof(ImportJob.TotalRows))]
    [MapperIgnoreTarget(nameof(ImportJob.SuccessCount))]
    [MapperIgnoreTarget(nameof(ImportJob.ErrorCount))]
    [MapperIgnoreTarget(nameof(ImportJob.Status))]
    [MapperIgnoreTarget(nameof(ImportJob.StartedAt))]
    [MapperIgnoreTarget(nameof(ImportJob.FinishedAt))]
    [MapperIgnoreTarget(nameof(ImportJob.Errors))]
    public partial ImportJob ToEntity(CreateImportJobDto dto);

    [MapperIgnoreSource(nameof(ImportJob.RowVersion))]
    [MapperIgnoreSource(nameof(ImportJob.CreatedOnAt))]
    [MapperIgnoreSource(nameof(ImportJob.UpdatedOnAt))]
    [MapperIgnoreSource(nameof(ImportJob.CreatedById))]
    [MapperIgnoreSource(nameof(ImportJob.UpdatedById))]
    [MapperIgnoreSource(nameof(ImportJob.CreatedBy))]
    [MapperIgnoreSource(nameof(ImportJob.UpdatedBy))]
    [MapperIgnoreSource(nameof(ImportJob.StoredFileName))]
    [MapperIgnoreSource(nameof(ImportJob.Errors))]
    public partial ImportJobResponseDto ToDto(ImportJob entity);

    [MapperIgnoreSource(nameof(ImportJob.RowVersion))]
    [MapperIgnoreSource(nameof(ImportJob.CreatedOnAt))]
    [MapperIgnoreSource(nameof(ImportJob.UpdatedOnAt))]
    [MapperIgnoreSource(nameof(ImportJob.CreatedById))]
    [MapperIgnoreSource(nameof(ImportJob.UpdatedById))]
    [MapperIgnoreSource(nameof(ImportJob.CreatedBy))]
    [MapperIgnoreSource(nameof(ImportJob.UpdatedBy))]
    [MapperIgnoreSource(nameof(ImportJob.StoredFileName))]
    public partial ImportJobDetailsResponseDto ToDetailsDto(ImportJob entity);

    [MapperIgnoreSource(nameof(ImportError.ImportJobId))]
    [MapperIgnoreSource(nameof(ImportError.ImportJob))]
    [MapperIgnoreSource(nameof(ImportError.CreatedOnAt))]
    [MapperIgnoreSource(nameof(ImportError.RowVersion))]
    public partial ImportErrorResponseDto ToDto(ImportError entity);

    [MapperIgnoreTarget(nameof(ImportJob.Id))]
    [MapperIgnoreTarget(nameof(ImportJob.RowVersion))]
    [MapperIgnoreTarget(nameof(ImportJob.CreatedOnAt))]
    [MapperIgnoreTarget(nameof(ImportJob.UpdatedOnAt))]
    [MapperIgnoreTarget(nameof(ImportJob.CreatedById))]
    [MapperIgnoreTarget(nameof(ImportJob.UpdatedById))]
    [MapperIgnoreTarget(nameof(ImportJob.CreatedBy))]
    [MapperIgnoreTarget(nameof(ImportJob.UpdatedBy))]
    [MapperIgnoreTarget(nameof(ImportJob.OriginalFileName))]
    [MapperIgnoreTarget(nameof(ImportJob.StoredFileName))]
    [MapperIgnoreTarget(nameof(ImportJob.MappingProfile))]
    [MapperIgnoreTarget(nameof(ImportJob.TotalRows))]
    [MapperIgnoreTarget(nameof(ImportJob.SuccessCount))]
    [MapperIgnoreTarget(nameof(ImportJob.ErrorCount))]
    [MapperIgnoreTarget(nameof(ImportJob.Status))]
    [MapperIgnoreTarget(nameof(ImportJob.StartedAt))]
    [MapperIgnoreTarget(nameof(ImportJob.FinishedAt))]
    [MapperIgnoreTarget(nameof(ImportJob.Errors))]
    public partial void UpdateEntity(UpdateImportJobDto dto, ImportJob entity);
}
