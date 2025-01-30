using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IUnitOfWork _uof;
        public OrderItemService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task<OrderItem> Get(int? Id)
        {
            return await _uof.OrderItemsRepository.Get(Id);
        }

        public async Task<DbSet<Product>> GetProductsAsync()
        {
            return await _uof.OrderItemsRepository.GetAllProducts();
        }

        public async Task<DbSet<Order>> GetOrdersAsync()
        {
            return await _uof.OrderItemsRepository.GetAllOrders();
        }
        public async Task<bool> Includes(int Id)
        {
            return await _uof.OrderItemsRepository.Includes(Id);
        }

        public Task<PagedResult<OrderItem>> List(int page, int pageSize)
        {
            return _uof.OrderItemsRepository.List(page, pageSize);
        }

        public async Task Save(OrderItem item)
        {
            await _uof.OrderItemsRepository.Save(item);
        }
        public async Task Delete(int Id)
        {
            await _uof.OrderItemsRepository.Delete(Id);

        }
    }
}
