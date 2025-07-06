using FinanceManager.Application.Categories.Dtos;
using Riok.Mapperly.Abstractions;

namespace FinanceManager.Api.ViewModels.Category.Mapping;

[Mapper]
public partial class CategoryViewModelMapper
{
    public partial CategoryViewModel ToViewModel(CategoryResponseDto dto);
    public partial CreateCategoryDto ToDto(CreateCategoryViewModel viewModel);
    public partial UpdateCategoryDto ToDto(UpdateCategoryViewModel viewModel);
}
