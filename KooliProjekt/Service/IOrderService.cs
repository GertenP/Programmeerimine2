using System.Collections.Generic;
using System.Threading.Tasks;
using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface IOrderService
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
        Task CreateOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(int id);
        Task<List<Order>> GetPagedOrdersAsync(int page, int pageSize);
    }
}
