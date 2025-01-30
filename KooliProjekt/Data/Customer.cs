namespace KooliProjekt.Data
{
    public class Customer : Entity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public string Address { get; set; }
    }
}
