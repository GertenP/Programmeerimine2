using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _uof;

        public OrderService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int id)
        {
            await _uof.OrdersRepository.Delete(id);
        }

        public async Task<Order> Get(int? id)
        {
            return await _uof.OrdersRepository.Get(id);
        }

        public async Task<DbSet<Customer>> GetCustomersAsync()
        {
            return await _uof.OrdersRepository.GetAllCustomers();
        }

        public async Task<bool> Includes(int id)
        {
            return await _uof.OrdersRepository.Includes(id);
        }

        public Task<PagedResult<Order>> List(int page, int pageSize)
        {
            return _uof.OrdersRepository.List(page, pageSize);
        }

        public async Task Save(Order order)
        {
            await _uof.OrdersRepository.Save(order);
        }
    

    }
}
