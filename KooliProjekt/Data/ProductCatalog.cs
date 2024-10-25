using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class ProductCatalog
    {
        public int Id { get; set; } // Primaarvõti

        [Required(ErrorMessage = "Kategooria nimi on kohustuslik.")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage = "Toote ID on kohustuslik.")]
        public int ProductId { get; set; }
    }
}
