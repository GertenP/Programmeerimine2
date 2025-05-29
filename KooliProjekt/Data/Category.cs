using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data;

public class Category: Entity
{
    public int Id { get; set; }
    
    [Required]
    public string Name { get; set; }
}