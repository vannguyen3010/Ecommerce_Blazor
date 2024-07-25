using Ecommerce_Library.Models;
using Ecommerce_Library.Responses;
using System.Net.Http;

namespace Ecommerce_Client.Services
{
    public class ClientServices(HttpClient httpClient) : IProductService, ICategoryService
    {
        private const string ProductBaseUrl = "api/product";
        private const string CategoryBaseUrl = "api/category";

        public Action? CategoryAction { get; set; }
        public List<Category> AllCategories { get; set; }
        public Action? ProductAction { get; set; }
        public List<Product> AllProducts { get; set; }
        public List<Product> FeaturedProducts { get; set; }

        //Product
        public async Task<ServiceResponse> AddProduct(Product model)
        {
            var response = await httpClient.PostAsync($"{ProductBaseUrl}/Add-Product", General.GenerateStringContent(General.SerializeObj(model)));

            var result = CheckResponse(response);
            if(!result.Flag)
                return result;

            var apiResponse = await ReadContent(response);
            await ClearAndGetAllProducts();
            return General.DeserializeJsonString<ServiceResponse>(apiResponse);
        }
        private async Task ClearAndGetAllProducts()
        {
            bool featuredProduct = true; //lấy sản phẩm nổi bật
            bool allProducts = false; //lấy tất cả sản phẩm
            AllProducts = null!; //tất cả sản phẩm hiện tại sẽ bị xóa
            FeaturedProducts = null!; // tất cả sản phẩm hiện tại sẽ bị xóa
            await GetAllProducts(featuredProduct); // lấy danh sách mới
            await GetAllProducts(allProducts);
        }

        public async Task GetAllProducts(bool featuredProducts)
        {
            //Nếu featuredProducts = true thì lấy danh sách sản phẩm nổi bật
            if (featuredProducts && FeaturedProducts is null)
            {
                FeaturedProducts = await GetProducts(featuredProducts);
                ProductAction?.Invoke();
                return;
            }

            //Nếu featuredProducts = false thì lấy danh sách sản phẩm
            if (!featuredProducts && AllProducts is null)
            {
                AllProducts = await GetProducts(featuredProducts);
                ProductAction?.Invoke();
                return;
            }
        }
        private async Task<List<Product>> GetProducts(bool featured)
        {
            var response = await httpClient.GetAsync($"{ProductBaseUrl}/All-Product?featured={featured}");
            var (flag, _) = CheckResponse(response);
            if (!flag) return null!;

            var result = await ReadContent(response);
            return (List<Product>?)General.DeserializeJsonStringList<Product>(result)!;
        }

        //Categories
        public async Task<ServiceResponse> AddCategory(Category model)
        {
            var response = await httpClient.PostAsync($"{CategoryBaseUrl}/Add-Category", General.GenerateStringContent(General.SerializeObj(model)));

            var result = CheckResponse(response);
            if (!result.Flag)
                return result;

            var apiResponse = await ReadContent(response);
            await ClearAndGetAllCategories();
            return General.DeserializeJsonString<ServiceResponse>(apiResponse);
        }

        public async Task GetAllCategories()
        {
            if (AllCategories is null)
            {
                var response = await httpClient.GetAsync($"{CategoryBaseUrl}");
                var (flag, _) = CheckResponse(response);
                if (!flag) return;

                var result = await ReadContent(response);
                AllCategories = (List<Category>?)General.DeserializeJsonStringList<Category>(result)!;
                CategoryAction?.Invoke();
            }
        }
        private async Task ClearAndGetAllCategories()
        {
            AllCategories = null!;
            await GetAllCategories();
        }

        //General method
        private static async Task<string> ReadContent(HttpResponseMessage response) => await response.Content.ReadAsStringAsync();
        private static ServiceResponse CheckResponse(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
                return new ServiceResponse(false, "Error occured, Try again later...");
            else
                return new ServiceResponse(true, null!);
        }

        Task IProductService.GetAllProducts(bool featuredProducts)
        {
            throw new NotImplementedException();
        }
    }
}
