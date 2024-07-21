using Ecommerce_Library.Contracts;
using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;
using Ecommerce_Server.Data;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce_Server.Repositories
{
    public class ProductRepository : IProduct
    {
        private readonly AppDbContext _appDbContext;

        public ProductRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        public async Task<ServiceResponse> AddProduct(Product model)
        {
            if (model is null) return new ServiceResponse(false, "Model is null");
            var (flag, message) = await CheckName(model.Name!);
            if(flag)
            {
                _appDbContext.Products.Add(model);
                await Commit();
                return new ServiceResponse(true, "Product Saved");
            }
            return new ServiceResponse(flag, message);
        }

        public async Task<List<Product>> GetAllProducts(bool featuredProducts)
        {
            if(featuredProducts)
                return await _appDbContext.Products.Where(x => x.Featured).ToListAsync();
            else
                return await _appDbContext.Products.ToListAsync();
        }

        private async Task<ServiceResponse> CheckName(string name)
        {
            var product = await _appDbContext.Products.FirstOrDefaultAsync(x => x.Name!.ToLower()!.Equals(name.ToLower()));
            return product is null ? new ServiceResponse(true, null!) : new ServiceResponse(false, "Product already exits");
        }
        private async Task Commit() => await _appDbContext.SaveChangesAsync();
    }
}
