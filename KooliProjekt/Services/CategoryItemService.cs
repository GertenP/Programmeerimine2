using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class CategoryItemService : ICategoryItemService
    {
        private readonly IUnitOfWork _uof;

        public CategoryItemService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int Id)
        {
            await _uof.CategoryRepository.Delete(Id);
        }

        public async Task<Category> Get(int Id)
        {
            return await _uof.CategoryRepository.Get(Id);
        }

        public async Task<Category> Get(int? Id)
        {
            return await _uof.CategoryRepository.Get(Id);
        }

        public async Task<bool> Includes(int Id)
        {
            return await _uof.CategoryRepository.Includes(Id);
        }

        public async Task<PagedResult<Category>> List(int page, int pageSize)
        {
            return await _uof.CategoryRepository.List(page, pageSize);
        }

        public async Task Save(Category category)
        {
            await _uof.CategoryRepository.Save(category);
        }
    }
}