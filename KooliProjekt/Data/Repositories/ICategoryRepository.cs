namespace KooliProjekt.Data.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        Task<Category> Get(int id);
        Task Save(Category category);
        Task Delete(int id);

        Task Delete(Category category);
        Task<PagedResult<Category>> List(int page, int pageSize);
    }
}
