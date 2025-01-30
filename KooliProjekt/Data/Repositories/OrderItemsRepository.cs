
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data.Repositories
{
    public class OrderItemsRepository : BaseRepository<OrderItem>, IOrderItemsRepository
    {
        public OrderItemsRepository(ApplicationDbContext context) : base(context)
        {
        }

    }
}
