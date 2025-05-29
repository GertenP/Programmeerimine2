using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task<T> Get(int? id);
        Task Save(T instance);
        Task Delete(T instance);
        Task Delete(int? id);
        Task<PagedResult<T>> List(int page, int pageSize);
        Task<bool> Includes(int Id);
        Task<DbSet<Product>> GetAllProducts();
        Task<DbSet<Customer>> GetAllCustomers();
        Task<DbSet<Category>> GetAllCategories();

    }

}