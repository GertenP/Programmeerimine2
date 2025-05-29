using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _uof;
        public CustomerService(IUnitOfWork uof)
        {
            _uof = uof;
        }

        public async Task Delete(int? Id)
        {
            await _uof.CustomersRepository.Delete(Id);
        }

        public async Task<Customer> Get(int? Id)
        {
            return await _uof.CustomersRepository.Get(Id);
        }

        public async Task<bool> Includes(int Id)
        {
            return await _uof.CustomersRepository.Includes(Id);
        }

        public Task<PagedResult<Customer>> List(int page, int pageSize)
        {
            return _uof.CustomersRepository.List(page, pageSize);
        }

        public async Task Save(Customer customer)
        {
            await _uof.CustomersRepository.Save(customer);
            
        }
    }
}