using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class UserData
    {
        public int Id { get; set; } // Primaarvõti

        [Required(ErrorMessage = "Kasutaja nimi on kohustuslik.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Hind on kohustuslik.")]
        public float Price { get; set; }

        [Required(ErrorMessage = "Kogus on kohustuslik.")]
        public int Amount { get; set; }

        [Required(ErrorMessage = "Toote ID on kohustuslik.")]
        public int ProductId { get; set; }

        public float Discount { get; set; }
    }
}
