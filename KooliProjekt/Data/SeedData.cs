namespace KooliProjekt.Data
{
    public class SeedData
    {
            public static void GenerateCategories(ApplicationDbContext context)
            {
                // Check if the Categories table is empty
                if (context.Categories.Any())
                {
                    return; // If data exists, don't do anything
                }

                // Create categories (without manually setting Id, assuming the database generates them automatically)
                var categories = new[]
                {
                new Category { Name = "Puuviljad" },
                new Category { Name = "Mööbel" },
                new Category { Name = "Kodukaubad" },
                new Category { Name = "Rõivad" },
                new Category { Name = "Spordi- ja vabaaja tooted" },
                new Category { Name = "Arvutite ja lisatarvikud" },
                new Category { Name = "Kogutavad esemed" },
                new Category { Name = "Raamatud ja ajakirjad" },
                new Category { Name = "Tervise- ja ilutooted" },
                new Category { Name = "Lastekaubad" }
            };

                // Add categories to the database context
                context.Categories.AddRange(categories);

                // Save changes to the database
                context.SaveChanges();
            }

        public static void GenerateProducts(ApplicationDbContext context)
        {
            // Kontrollige, kas tooted on juba olemas
            if (context.Products.Any())
            {
                return; // Kui tooted on juba olemas, siis ei tee midagi
            }

            // Loome tooted koos kategooriate ja kirjeldustega
            var products = new[]
            {
                new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, Discount = 10 },
                new Product { Name = "Banaan", Description = "Looduslik roheline", Price = 1.49m, CategoryId = 1, Discount = 5 },
                new Product { Name = "Diivan", Description = "", Price = 299.99m, CategoryId = 2, Discount = 20 },
                new Product { Name = "Kohvimasin", Description = "Automaatne espresso", Price = 149.99m, CategoryId = 3, Discount = 15 },
                new Product { Name = "T-särk", Description = "Võidusõidustiil", Price = 19.99m, CategoryId = 4, Discount = 0 },
                new Product { Name = "Jalgratas", Description = "Mugav ja kiire", Price = 349.99m, CategoryId = 5, Discount = 10 },
                new Product { Name = "Arvutihiir", Description = "Ergonoomiline", Price = 29.99m, CategoryId = 6, Discount = 5 },
                new Product { Name = "Raamat", Description = "Romaan 2024", Price = 14.99m, CategoryId = 8, Discount = 0 },
                new Product { Name = "Shampoo", Description = "Kuse lõhn mees", Price = 9.99m, CategoryId = 9, Discount = 20 },
                new Product { Name = "Lastepall", Description = "Pehme ja turvaline", Price = 12.99m, CategoryId = 10, Discount = 10 }
            };

            // Lisame tooted andmebaasi
            context.Products.AddRange(products);

            // Salvestame muudatused andmebaasi
            context.SaveChanges();
        }
        public static void GenerateCustomers(ApplicationDbContext context)
        {
            // Kontrollige, kas kliendid on juba olemas
            if (context.Customers.Any())
            {
                return; // Kui kliendid on juba olemas, siis ei tee midagi
            }

            // Loome 10 kliendi andmed
            var customers = new[]
            {
                new Customer { Name = "Markus Tamm", Email = "markus.tamm@example.com", Phone = 123456789, Address = "Tartu, Turu 5" },
                new Customer { Name = "Anna Kukk", Email = "anna.kukk@example.com", Phone = 234567890, Address = "Tallinn, Põllu 7" },
                new Customer { Name = "Jaanus Vaher", Email = "jaanus.vaher@example.com", Phone = 345678901, Address = "Pärnu, Jõe 12" },
                new Customer { Name = "Liis Mänd", Email = "liis.mänd@example.com", Phone = 456789012, Address = "Tartu, Pargi 1" },
                new Customer { Name = "Olev Kaar", Email = "olev.kaar@example.com", Phone = 567890123, Address = "Narva, Vabaduse 14" },
                new Customer { Name = "Kristel Lille", Email = "kristel.lille@example.com", Phone = 678901234, Address = "Tallinn, Roosi 3" },
                new Customer { Name = "Raul Kivi", Email = "raul.kivi@example.com", Phone = 789012345, Address = "Pärnu, Jõe 9" },
                new Customer { Name = "Eeva Sõber", Email = "eeva.sõber@example.com", Phone = 890123456, Address = "Tartu, Kooli 4" },
                new Customer { Name = "Priit Õun", Email = "priit.õun@example.com", Phone = 901234567, Address = "Tallinn, Ülikooli 8" },
                new Customer { Name = "Kertu Mets", Email = "kertu.mets@example.com", Phone = 123123123, Address = "Pärnu, Mere 6" }
            };

            // Lisame kliendid andmebaasi
            context.Customers.AddRange(customers);

            // Salvestame muudatused andmebaasi
            context.SaveChanges();
        }

        public static void GenerateOrders(ApplicationDbContext context)
        {
            // Kontrollige, kas tellimused on juba olemas
            if (context.Orders.Any())
            {
                return; // Kui tellimused on juba olemas, siis ei tee midagi
            }

            // Loome tellimused, igaühel oma CustomerId, Date ja staatus
            var orders = new[]
            {
                new Order { CustomerId = 1, Date = DateTime.Now, Staatus = "Ootel" },
                new Order { CustomerId = 2, Date = DateTime.Now, Staatus = "Täidetud" },
                new Order { CustomerId = 3, Date = DateTime.Now, Staatus = "Ootel" },
                new Order { CustomerId = 4, Date = DateTime.Now, Staatus = "Tühistatud" },
                new Order { CustomerId = 5, Date = DateTime.Now, Staatus = "Täidetud" },
                new Order { CustomerId = 6, Date = DateTime.Now, Staatus = "Ootel" },
                new Order { CustomerId = 7, Date = DateTime.Now, Staatus = "Täidetud" },
                new Order { CustomerId = 8, Date = DateTime.Now, Staatus = "Ootel" },
                new Order { CustomerId = 9, Date = DateTime.Now, Staatus = "Täidetud" },
                new Order { CustomerId = 10, Date = DateTime.Now, Staatus = "Tühistatud" }
            };

            // Lisame tellimused andmebaasi
            context.Orders.AddRange(orders);

            // Salvestame muudatused andmebaasi
            context.SaveChanges();
        }

        public static void GenerateOrderItems(ApplicationDbContext context)
        {
            // Kontrollime, kas OrderItems on juba olemas
            if (context.OrderItems.Any())
            {
                return; // Kui OrderItems on juba olemas, siis ei tee midagi
            }

            // Eeldame, et tellimused ja tooted on olemas

            var order1 = context.Orders.FirstOrDefault(); // Esimene tellimus
            var order2 = context.Orders.Skip(1).Take(1).FirstOrDefault(); // Teine tellimus
            var order3 = context.Orders.Skip(2).Take(1).FirstOrDefault(); // Kolmas tellimus
            var order4 = context.Orders.Skip(3).Take(1).FirstOrDefault(); // Neljas tellimus
            var order5 = context.Orders.Skip(4).Take(1).FirstOrDefault(); // Viies tellimus
            var order6 = context.Orders.Skip(5).Take(1).FirstOrDefault(); // Kuues tellimus
            var order7 = context.Orders.Skip(6).Take(1).FirstOrDefault(); // Seitsmes tellimus
            var order8 = context.Orders.Skip(7).Take(1).FirstOrDefault(); // Kaheksas tellimus
            var order9 = context.Orders.Skip(8).Take(1).FirstOrDefault(); // Üheksas tellimus
            var order10 = context.Orders.Skip(9).Take(1).FirstOrDefault(); // Kümnes tellimus

            var product1 = context.Products.FirstOrDefault(); // Esimene toode
            var product2 = context.Products.Skip(1).Take(1).FirstOrDefault(); // Teine toode
            var product3 = context.Products.Skip(2).Take(1).FirstOrDefault(); // Kolmas toode
            var product4 = context.Products.Skip(3).Take(1).FirstOrDefault(); // Neljas toode
            var product5 = context.Products.Skip(4).Take(1).FirstOrDefault(); // Viies toode
            var product6 = context.Products.Skip(5).Take(1).FirstOrDefault(); // Kuues toode
            var product7 = context.Products.Skip(6).Take(1).FirstOrDefault(); // Seitsmes toode
            var product8 = context.Products.Skip(7).Take(1).FirstOrDefault(); // Kaheksas toode
            var product9 = context.Products.Skip(8).Take(1).FirstOrDefault(); // Üheksas toode
            var product10 = context.Products.Skip(9).Take(1).FirstOrDefault(); // Kümnes toode

            // Loome OrderItemid (seose tellimuse ja toodete vahel)
            var orderItems = new[]
            {
                new OrderItem { OrderId = order1?.Id ?? 0, ProductId = product1?.Id ?? 0, Quantity = 2 }, // Tellimus 1, Toode 1, Kogus 2
                new OrderItem { OrderId = order2?.Id ?? 0, ProductId = product2?.Id ?? 0, Quantity = 1 }, // Tellimus 2, Toode 2, Kogus 1
                new OrderItem { OrderId = order3?.Id ?? 0, ProductId = product3?.Id ?? 0, Quantity = 3 }, // Tellimus 3, Toode 3, Kogus 3
                new OrderItem { OrderId = order4?.Id ?? 0, ProductId = product4?.Id ?? 0, Quantity = 1 }, // Tellimus 4, Toode 4, Kogus 1
                new OrderItem { OrderId = order5?.Id ?? 0, ProductId = product5?.Id ?? 0, Quantity = 2 }, // Tellimus 5, Toode 5, Kogus 2
                new OrderItem { OrderId = order6?.Id ?? 0, ProductId = product6?.Id ?? 0, Quantity = 5 }, // Tellimus 6, Toode 6, Kogus 5
                new OrderItem { OrderId = order7?.Id ?? 0, ProductId = product7?.Id ?? 0, Quantity = 1 }, // Tellimus 7, Toode 7, Kogus 1
                new OrderItem { OrderId = order8?.Id ?? 0, ProductId = product8?.Id ?? 0, Quantity = 4 }, // Tellimus 8, Toode 8, Kogus 4
                new OrderItem { OrderId = order9?.Id ?? 0, ProductId = product9?.Id ?? 0, Quantity = 2 }, // Tellimus 9, Toode 9, Kogus 2
                new OrderItem { OrderId = order10?.Id ?? 0, ProductId = product10?.Id ?? 0, Quantity = 3 } // Tellimus 10, Toode 10, Kogus 3
            };

            // Lisame OrderItems andmebaasi
            context.OrderItems.AddRange(orderItems);

            // Salvestame muudatused andmebaasi
            context.SaveChanges();
        }

    }
}

