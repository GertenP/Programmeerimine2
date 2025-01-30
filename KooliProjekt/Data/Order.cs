namespace KooliProjekt.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public List<OrderItem> OrderProducts { get; set; } = new List<OrderItem>();
    

        public string Staatus { get; set; }
    }
}

    
