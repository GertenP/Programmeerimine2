namespace KooliProjekt.Data.Repositories
{
    public interface IUnitOfWork
    {
        Task BeginTransaction();
        Task Commit();
        Task Rollback();

        ICategoryRepository CategoryRepository { get; }
        ICustomersRepository CustomersRepository { get; }
        IOrderItemsRepository OrderItemsRepository { get; }
        IOrdersRepository OrdersRepository { get; }
        IProductsRepository ProductsRepository { get; }
        
    }
}
