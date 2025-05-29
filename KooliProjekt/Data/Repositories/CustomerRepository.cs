using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class CustomersRepository : BaseRepository<Customer>, ICustomersRepository
    {
        public CustomersRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}