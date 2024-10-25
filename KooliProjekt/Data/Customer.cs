using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Customer
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int Id { get; set; }
    }
}
