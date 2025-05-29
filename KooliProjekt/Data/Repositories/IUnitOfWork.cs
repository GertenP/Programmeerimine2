namespace KooliProjekt.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();

        ICategoryRepository CategoryRepository { get; }
        ICustomersRepository CustomersRepository { get; }
        IProductsRepository ProductsRepository { get; }
        
    }
}