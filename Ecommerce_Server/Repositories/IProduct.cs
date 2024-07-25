using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;

namespace Ecommerce_Server.Repositories
{
    public interface IProduct
    {
        Task<ServiceResponse> AddProduct(Product model);
        Task<List<Product>> GetAllProducts(bool featuredProducts);
    }
}
