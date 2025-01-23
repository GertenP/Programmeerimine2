using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Order> Get(int? id)
        {
            return await _context.Orders.FindAsync(id);
        }

        public async Task<IList<Customer>> GetCustomersAsync()
        {
            return _context.Customers.ToList();
        }

        public async Task<bool> Includes(int id)
        {
            return await _context.Orders.AnyAsync(o => o.Id == id);
        }

        public Task<PagedResult<Order>> List(int page, int pageSize)
        {
            return _context.Orders.GetPagedAsync(page, pageSize);
        }

        public async Task Save(Order order)
        {
            if (order.Id == 0)
            {
                _context.Orders.Add(order);
            }
            else
            {
                _context.Orders.Update(order);
            }
            await _context.SaveChangesAsync();
        }
    

    }
}
