using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Tootenimi on kohustuslik")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Required(ErrorMessage = "Tootehind on kohustuslik")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Kategooria valimine on kohustuslik")]
        public int CategoryId { get; set; }
        // vaikimisi null
        [Range(0, 100, ErrorMessage = "Soodustus peab olema 0-100% vahemikus")]
        public int? Discount { get; set; }
    }
}
