namespace KooliProjekt.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public String Description { get; set; }
        public double Price { get; set; }
        public int CategoryId { get; set; }
        public double Discount { get; set; }
    }
}
