using FinanceManager.Application.Tags.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.Tag.Mapping;

[Mapper]
public partial class TagViewModelMapper
{
    public partial TagViewModel ToViewModel(TagResponseDto dto);
    public partial CreateTagDto ToDto(CreateTagViewModel viewModel);
    public partial UpdateTagDto ToDto(UpdateTagViewModel viewModel);
}
