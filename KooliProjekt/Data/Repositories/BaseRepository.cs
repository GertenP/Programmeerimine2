using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public abstract class BaseRepository<T> where T : Entity
    {
        protected ApplicationDbContext DbContext { get; }

        public BaseRepository(ApplicationDbContext context)
        {
            DbContext = context;
        }

        public virtual async Task<T> Get(int? id)
        {
            return await DbContext.Set<T>().FindAsync(id);
        }

        public virtual async Task<PagedResult<T>> List(int page, int pageSize)
        {
            return await DbContext.Set<T>()
                .OrderByDescending(x => x.Id)
                .GetPagedAsync(page, pageSize);
        }

        public virtual async Task Save(T item)
        {
            if (item.Id == 0)
            {
                DbContext.Set<T>().Add(item);
            }
            else
            {
                DbContext.Set<T>().Update(item);
            }

            await DbContext.SaveChangesAsync();
        }

        public virtual async Task Delete(int? id)
        {
            await DbContext.Set<T>()
                .Where(item => item.Id == id)
                .ExecuteDeleteAsync();
        }

        public virtual async Task Delete(T t)
        {
            DbContext.Set<T>().Remove(t);
            await DbContext.SaveChangesAsync();
        }

        public virtual async Task<bool> Includes(int Id)
        {
            return await DbContext.Set<T>().AnyAsync(x => x.Id == Id);
        }

        public virtual async Task<DbSet<Product>> GetAllProducts()
        {
            return DbContext.Products;
        }
        

        public virtual async Task<DbSet<Customer>> GetAllCustomers()
        {
            return DbContext.Customers;
        }

        public virtual async Task<DbSet<Category>> GetAllCategories()
        {
            return DbContext.Categories;
        }
    }
}