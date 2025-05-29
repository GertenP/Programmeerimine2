using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data;

public class Customer   
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Email { get; set; }
}