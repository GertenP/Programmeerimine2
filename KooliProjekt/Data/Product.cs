using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data;

public class Product: Entity
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    [Required]
    public int CategoryId { get; set; }
    public int DiscountPercentage { get; set; }
}