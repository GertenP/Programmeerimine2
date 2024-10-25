using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Tellimuse staatuse määramine on kohustuslik.")]
        public string Status { get; set; }

        public int ClientId { get; set; }

        // Links to the products in the order
        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
