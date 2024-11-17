using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class OrderItem
    {
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; } // Võõrvõti

        [Required]
        public int ProductId { get; set; } // Võõrvõti

        [Required]
        public double Price { get; set; } // Salvestatud hind ostu hetkel

        [Required]
        public double Discount { get; set; } // Salvestatud allahindlus ostu hetkel

        [Required]
        public int Quantity { get; set; } // Ostetud kogus
    }
}
