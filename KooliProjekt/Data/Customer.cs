using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Customer
    {
        [Required(ErrorMessage = "Nimi on kohustuslik.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Kasutaja ID on kohustuslik.")]
        public int Id { get; set; }
    }
}
