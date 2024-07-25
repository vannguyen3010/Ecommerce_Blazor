using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;

namespace Ecommerce_Server.Repositories
{
    public interface ICategory
    {
        Task<ServiceResponse> AddCategory(Category model);
        Task<List<Category>> GetAllCategories();
    }
}
