using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Product
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Toote nimi on kohustuslik.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Toote kirjeldus on kohustuslik.")]
        public string Description { get; set; }

        public string Photo { get; set; }

        [Required(ErrorMessage = "Hind on kohustuslik.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Hind peab olema suurem kui 0.")]
        public float Price { get; set; }

        public float Discount { get; set; }
    }
}
