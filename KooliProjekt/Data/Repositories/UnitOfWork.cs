namespace KooliProjekt.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context,
        ICategoryRepository categoryRepository,
        ICustomersRepository customersRepository,
        IOrderItemsRepository orderItemsRepository,
        IOrdersRepository ordersRepository,
        IProductsRepository productsRepository)
        
        {
            _context = context;

            CategoryRepository = categoryRepository;
            CustomersRepository = customersRepository;
            OrderItemsRepository = orderItemsRepository;
            OrdersRepository = ordersRepository;
            ProductsRepository = productsRepository;
        }

        public ICategoryRepository CategoryRepository { get; }
        public ICustomersRepository CustomersRepository { get; }
        public IOrderItemsRepository OrderItemsRepository { get; }
        public IOrdersRepository OrdersRepository { get; }
        public IProductsRepository ProductsRepository { get; }



        public async Task BeginTransaction()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task Commit()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public async Task Rollback()
        {
            await _context.Database.RollbackTransactionAsync();
        }
    }
}