using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public interface IOrderService
    {
        Task<PagedResult<Order>> List(int page, int pageSize);
        Task<DbSet<Customer>> GetCustomersAsync();
        Task<Order> Get(int? id);
        Task Save(Order order);
        Task Delete(int id);

        Task<bool> Includes(int id);

    }
}
