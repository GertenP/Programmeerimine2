using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> List(int page, int pageSize);
        Task<List<Category>> GetCategoriesAsync();
        Task<List<Product>> GetProductsAsync();

        Task<Product> Get(int id);
        Task Save (Product product);
        Task Delete (int id);
    }
}
