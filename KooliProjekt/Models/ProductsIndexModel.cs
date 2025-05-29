using KooliProjekt.Data;
using KooliProjekt.Search;

namespace KooliProjekt.Models;

public class ProductsIndexModel
{
    public ProductSearch Search { get; set; }
    public PagedResult<Product> Data { get; set; }
}