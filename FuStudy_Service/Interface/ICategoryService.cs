using FuStudy_Model.DTO.Request;
using FuStudy_Model.DTO.Response;

namespace FuStudy_Service.Interface;

public interface ICategoryService
{
    Task<IEnumerable<CategoryResponse>> GetAllCategories();

    Task<CategoryResponse> GetCategoryByIdAsync(long id);

    Task<CategoryResponse> CreateCategoryAsync(CategoryRequest categoryRequest);
        
    Task<CategoryResponse> UpdateCategoryAsync(CategoryRequest categoryRequest, long categoryId);
        
    Task<bool> DeleteCategoryAsync(long questionId);

}