using KooliProjekt.Data;

namespace KooliProjekt.Services
{
    public interface ICustomerService
    {
        Task<PagedResult<Customer>> List(int page, int pageSize);
        Task<Customer> Get(int? Id);
        Task Save(Customer customer);
        Task Delete(int? Id);
        Task<bool> Includes(int Id);

    }
}