using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class CategoryItemService : ICategoryItemService
    {
        private readonly ApplicationDbContext _context;

        public CategoryItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int Id)
        {
            var category = await _context.Categories.FindAsync(Id);
            if (category != null)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Category> Get(int Id)
        {
            return await _context.Categories.FindAsync(Id);
        }

        public async Task<Category> Get(int? Id)
        {
            return await _context.Categories.FindAsync(Id);
        }

        public Task<bool> Includes(int Id)
        {
            return _context.Categories.AnyAsync(product => product.Id == Id);
        }

        public async Task<PagedResult<Category>> List(int page, int pageSize, CategorySearch search = null)
        {
            var query = _context.Categories.AsQueryable();
            search = search ?? new CategorySearch();
            if (!string.IsNullOrWhiteSpace((search.Keyword)))
            {
                query = query.Where(product => product.Name.Contains(search.Keyword));
            }

            return await query.OrderBy(list => list.Name).GetPagedAsync(page, pageSize);
        }

        public async Task Save(Category category)
        {
            if (category.Id == 0)
            {
                _context.Categories.Add(category);
            }
            else
            {
                _context.Categories.Update(category);
            }

            await _context.SaveChangesAsync();
        }
    }
}