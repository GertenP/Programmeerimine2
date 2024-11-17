using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KooliProjekt.Data
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; } // Võõrvõti, mis viitab kliendile

        public string Status { get; set; } = "Pending"; // Vaikimisi väärtus

        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>(); // Tellimuse üksused

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now; // Väärtustatakse alati tänase kuupäevaga
    }
}
