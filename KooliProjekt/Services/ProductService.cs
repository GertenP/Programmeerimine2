using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IList<Category>> GetCategoriesAsync()
        {
            return _context.Categories.ToList();

        }

        public async Task<IList<Product>> GetProductsAsync()
        {
            return _context.Products.ToList();
        }

        public async Task<PagedResult<Product>> List(int page, int pageSize, ProductSearch search = null)
        {
            var query = _context.Products.AsQueryable();
            search = search ?? new ProductSearch();
            if (!string.IsNullOrWhiteSpace((search.Keyword)))
            {
                query = query.Where(product => product.Name.Contains(search.Keyword));
            }

            return await query.OrderBy(list => list.Name).GetPagedAsync(page, pageSize);
        }

        public async Task<Product> Get(int? id)
        {
            return await _context.Products.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task Save(Product product)
        {
            if (product.Id == 0)
            {
                _context.Add(product);
            }
            else
            {
                _context.Update(product);
            }

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int Id)
        {
            var product = await _context.Products.FindAsync(Id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }

        }

        public async Task<bool> Includes(int Id)
        {
            return await _context.Products.AnyAsync(product => product.Id == Id);
        }
    }
}