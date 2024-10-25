using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public DbSet<Product> Invoices { get; set; }  // Using DbSet, although unconventional here
        public int ClientId { get; set; }
    }
}
