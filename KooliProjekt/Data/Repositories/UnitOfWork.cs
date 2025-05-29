namespace KooliProjekt.Data.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context,
            ICategoryRepository categoryRepository,
            ICustomersRepository customersRepository,
            IProductsRepository productsRepository)
        
        {
            _context = context;

            CategoryRepository = categoryRepository;
            CustomersRepository = customersRepository;
            ProductsRepository = productsRepository;
        }

        public ICategoryRepository CategoryRepository { get; }
        public ICustomersRepository CustomersRepository { get; }
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