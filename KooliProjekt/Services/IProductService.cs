using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public interface IProductService
    {
        Task<PagedResult<Product>> List(int page, int pageSize);
        Task<IList<Category>> GetCategoriesAsync();
        Task<IList<Product>> GetProductsAsync();

        Task<Product> Get(int? id);
        Task Save (Product product);
        Task Delete (int id);

        Task<bool> Includes(int id);

    }
}