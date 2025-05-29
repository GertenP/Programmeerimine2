using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models;

public class CategoriesIndexModel
{
    public CategorySearch Search { get; set; }
    public PagedResult<Category> Data { get; set; }
}