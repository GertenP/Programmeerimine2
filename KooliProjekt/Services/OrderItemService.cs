using KooliProjekt.Data;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class OrderItemService : IOrderItemService
    {
        private readonly ApplicationDbContext _context;
        public OrderItemService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderItem> Get(int? Id)
        {
            var orderitem = await _context.OrderItems.FindAsync(Id);
            if (orderitem != null)
            {
                return orderitem;
            }
            return null;
        }

        public async Task<DbSet<Product>> GetProductsAsync()
        {
            return _context.Products;
        }

        public async Task<DbSet<Order>> GetOrdersAsync()
        {
            return _context.Orders;
        }
        public async Task<bool> Includes(int Id)
        {
            return await _context.OrderItems.AnyAsync(x => x.Id == Id);
        }

        public Task<PagedResult<OrderItem>> List(int page, int pageSize)
        {
            return _context.OrderItems.GetPagedAsync(page, pageSize);
        }

        public async Task Save(OrderItem item)
        {
            if (item.Id == 0)
            {
                await _context.OrderItems.AddAsync(item);
            }
            else
            {
                _context.OrderItems.Update(item);
            }
            await _context.SaveChangesAsync();
        }
        public async Task Delete(int Id)
        {
            var orderitem = await _context.OrderItems.FindAsync(Id);
            if (orderitem != null)
            {
                _context.OrderItems.Remove(orderitem);
                await _context.SaveChangesAsync();
            }

        }
    }
}
