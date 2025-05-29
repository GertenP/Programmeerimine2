using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class ProductsRepository : BaseRepository<Product>, IProductsRepository
    {
        public ProductsRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}