namespace KooliProjekt.Data
{
    public class OrderItem : Entity
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public int Discount { get; set; }

        // Funktsioon, et määrata hind ja soodustus toote põhjal
        public void SetProductDetails(IEnumerable<Product> products)
        {
            var product = products.FirstOrDefault(p => p.Id == ProductId);
            if (product != null)
            {
                Price = product.Price * Quantity;        // Määrame hinna toote põhjal
                Discount = product.Discount;  // Määrame soodustuse toote põhjal
            }
        }
    }
}
