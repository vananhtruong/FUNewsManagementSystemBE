using BusinessObject.Entities;
using BusinessObject.Model;

namespace FUNews.BLL.Services
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<CategoryReturnDTO?> GetCategoryByIdAsync(short id);
        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(short id);
        Task<IEnumerable<Category>> GetActiveCategoriesAsync();
    }
}
