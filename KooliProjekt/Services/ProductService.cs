using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _uof;

        public ProductService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<DbSet<Category>> GetAllCategories()
        {
            return await _uof.ProductsRepository.GetAllCategories();

        }

        public async Task<DbSet<Product>> GetAllProducts()
        {
            return await _uof.ProductsRepository.GetAllProducts();
        }

        public async Task<PagedResult<Product>> List(int page, int pageSize)
        {
            return await _uof.ProductsRepository.List(page, pageSize);
        }

        public async Task<Product> Get(int? id)
        {
            return await _uof.ProductsRepository.Get(id);
        }

        public async Task Save(Product product)
        {
           await _uof.ProductsRepository.Save(product);
        }

        public async Task Delete(int Id)
        {
            await _uof.ProductsRepository.Delete(Id);
        }

        public async Task<bool> Includes(int Id)
        {
            return await _uof.ProductsRepository.Includes(Id);
        }
    }
}