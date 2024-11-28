﻿namespace KooliProjekt.Data
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        // vaikimisi null
        public int Discount { get; set; } = 0;
    }
}
