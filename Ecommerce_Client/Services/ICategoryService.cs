using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;

namespace Ecommerce_Client.Services
{
    public interface ICategoryService
    {
        Action? CategoryAction { get; set; }
        Task<ServiceResponse> AddCategory(Category model);
        Task GetAllCategories();
        List<Category> AllCategories { get; set; }
    }
}
