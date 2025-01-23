using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public interface IOrderItemService
    {
        Task<PagedResult<OrderItem>> List(int page,  int pageSize);
        Task<IList<Product>> GetProductsAsync();
        Task<IList<Order>> GetOrdersAsync();

        Task Save(OrderItem item);
        Task Delete(int Id);

        Task<OrderItem> Get(int? Id);
        Task<bool> Includes(int Id);
    }
}

