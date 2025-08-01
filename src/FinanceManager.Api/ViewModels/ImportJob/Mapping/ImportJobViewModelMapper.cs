using System.Text.Json;
using FinanceManager.Application.ImportJobs.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.ImportJob.Mapping;

[Mapper]
public partial class ImportJobViewModelMapper
{
    public CreateImportJobDto ToCreateDto(string originalFileName, JsonDocument mappingProfile, bool notifyOnCompletion)
        => new()
        {
            OriginalFileName = originalFileName,
            MappingProfile = mappingProfile,
            NotifyOnCompletion = notifyOnCompletion
        };

    public partial UpdateImportJobDto ToUpdateDto(UpdateImportJobViewModel viewModel);

    public partial ImportJobViewModel ToViewModel(ImportJobResponseDto dto);

    public partial ImportJobDetailsViewModel ToDetailsViewModel(ImportJobDetailsResponseDto dto);


    // Nested mapping
    public partial ImportErrorViewModel ToViewModel(ImportErrorResponseDto responseDto);
}
