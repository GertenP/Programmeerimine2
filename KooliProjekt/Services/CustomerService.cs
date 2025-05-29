using KooliProjekt.Data;
using KooliProjekt.Search;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        public CustomerService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task Delete(int? Id)
        {
            var customer = await _context.Customers.FindAsync(Id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Customer> Get(int? Id)
        {
            return await _context.Customers.FindAsync(Id);
        }

        public async Task<bool> Includes(int Id)
        {
            return await _context.Customers.AnyAsync(c => c.Id == Id);
        }

        public async Task<PagedResult<Customer>> List(int page, int pageSize, CustomerSearch search = null)
        {
            var query = _context.Customers.AsQueryable();
            search = search ?? new CustomerSearch();
            if (!string.IsNullOrWhiteSpace((search.Keyword)))
            {
                query = query.Where(product => product.Name.Contains(search.Keyword));
            }

            return await query.OrderBy(list => list.Name).GetPagedAsync(page, pageSize);
        }

        public async Task Save(Customer customer)
        {
            if (customer.Id == 0)
            {
                _context.Customers.Add(customer);
            } else
            {
                _context.Customers.Update(customer);
            }
            await _context.SaveChangesAsync();
            
        }
    }
}