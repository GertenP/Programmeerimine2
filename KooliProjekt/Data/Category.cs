using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data;

public class Category
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
}