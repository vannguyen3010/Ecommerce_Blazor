using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;

namespace Ecommerce_Library.Contracts
{
    public interface IProduct
    {
        Task<ServiceResponse> AddProduct(Product model);
        Task<List<Product>> GetAllProducts(bool featuredProducts);
    }
}
