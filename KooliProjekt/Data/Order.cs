namespace KooliProjekt.Data
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;

        public List<OrderItem> OrderProducts { get; set; } = new List<OrderItem>();

        public decimal Total
        {   
            get
            {
                return OrderProducts.Sum(product => product.Price * product.Quantity);
            }
        }

        public string Staatus { get; set; }
    }
}

    
