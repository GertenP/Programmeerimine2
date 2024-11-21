namespace KooliProjekt.Data
{
    public static class SeedData
    {
        public static void Generate(ApplicationDbContext context)
        {
#if DEBUG
            // Kategooriate genereerimine
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics", Description = "Phones, laptops, and gadgets." },
                    new Category { Name = "Clothing", Description = "Clothes for everyone." },
                    new Category { Name = "Furniture", Description = "Furniture for your home." },
                    new Category { Name = "Books", Description = "Books for all ages." },
                    new Category { Name = "Toys", Description = "Toys for kids." },
                    new Category { Name = "Home Appliances", Description = "Appliances for your home." },
                    new Category { Name = "Sports Equipment", Description = "Gear for sports." },
                    new Category { Name = "Beauty & Health", Description = "Beauty and health products." },
                    new Category { Name = "Groceries", Description = "Food and drinks." },
                    new Category { Name = "Automotive", Description = "Car parts and accessories." }
                };
                context.Categories.AddRange(categories);
                context.SaveChanges();
            }

            // Kontrollime ja loome tooted
            if (!context.Products.Any())
            {
                var categoryList = context.Categories.ToList(); 

                var products = new List<Product>
                {
                    new Product { Name = "Nutitelefon", Description = "Uusim mudel koos 5G toega", Price = 699.99, Discount = 10.0, CategoryId = categoryList.First(c => c.Name == "Electronics").Id },
                    new Product { Name = "Tolmuimeja", Description = "Võimas ja vaikne kodutolmuimeja", Price = 199.99, Discount = 15.0, CategoryId = categoryList.First(c => c.Name == "Home Appliances").Id },
                    new Product { Name = "Konstruktorikomplekt", Description = "Loominguline mängukomplekt lastele", Price = 49.99, Discount = 5.0, CategoryId = categoryList.First(c => c.Name == "Toys").Id },
                    new Product { Name = "Kohvimasin", Description = "Automaatne kohvimasin latte ja cappuccino jaoks", Price = 299.99, Discount = 20.0, CategoryId = categoryList.First(c => c.Name == "Home Appliances").Id },
                    new Product { Name = "E-luger", Description = "Kerge ja kompaktne e-raamatute luger", Price = 129.99, Discount = 10.0, CategoryId = categoryList.First(c => c.Name == "Books").Id },
                    new Product { Name = "Bluetooth kõrvaklapid", Description = "Juhtmevabad kõrvaklapid mürasummutusega", Price = 89.99, Discount = 5.0, CategoryId = categoryList.First(c => c.Name == "Electronics").Id },
                    new Product { Name = "Laste lauamäng", Description = "Põnev ja hariv lauamäng kogu perele", Price = 29.99, Discount = 0.0, CategoryId = categoryList.First(c => c.Name == "Toys").Id },
                    new Product { Name = "Õpik: Programmeerimise alused", Description = "Õpi programmeerimise põhitõdesid Java keeles", Price = 39.99, Discount = 15.0, CategoryId = categoryList.First(c => c.Name == "Books").Id },
                    new Product { Name = "Nutivõru", Description = "Aktiivsuse jälgija südame löögisageduse mõõtmisega", Price = 59.99, Discount = 10.0, CategoryId = categoryList.First(c => c.Name == "Electronics").Id },
                    new Product { Name = "Vannitoakomplekt", Description = "Stiilne komplekt hambaharjahoidja ja seebialusega", Price = 24.99, Discount = 5.0, CategoryId = categoryList.First(c => c.Name == "Home Appliances").Id }
                };

                context.Products.AddRange(products);
                context.SaveChanges(); 
            }

            if (!context.Customers.Any())
            {
                var customers = new List<Customer>
                {
                    new Customer { Name = "John Doe", Email = "john.doe@example.com" },
                    new Customer { Name = "Jane Smith", Email = "jane.smith@example.com" },
                    new Customer { Name = "Alice Johnson", Email = "alice.johnson@example.com" },
                    new Customer { Name = "Bob Brown", Email = "bob.brown@example.com" },
                    new Customer { Name = "Charlie Davis", Email = "charlie.davis@example.com" },
                    new Customer { Name = "David Evans", Email = "david.evans@example.com" },
                    new Customer { Name = "Emma Clark", Email = "emma.clark@example.com" },
                    new Customer { Name = "Frank Harris", Email = "frank.harris@example.com" },
                    new Customer { Name = "Grace Martin", Email = "grace.martin@example.com" },
                    new Customer { Name = "Henry Lee", Email = "henry.lee@example.com" }
                };
                context.Customers.AddRange(customers);
                context.SaveChanges();
            }

            if (!context.Orders.Any())
            {
                var orders = new List<Order>
                {
                    new Order { CustomerId = 1, Status = "Pending", OrderDate = DateTime.Now.AddDays(-1) },
                    new Order { CustomerId = 2, Status = "Shipped", OrderDate = DateTime.Now.AddDays(-2) },
                    new Order { CustomerId = 3, Status = "Delivered", OrderDate = DateTime.Now.AddDays(-3) },
                    new Order { CustomerId = 4, Status = "Pending", OrderDate = DateTime.Now.AddDays(-4) },
                    new Order { CustomerId = 5, Status = "Shipped", OrderDate = DateTime.Now.AddDays(-5) },
                    new Order { CustomerId = 6, Status = "Cancelled", OrderDate = DateTime.Now.AddDays(-6) },
                    new Order { CustomerId = 7, Status = "Pending", OrderDate = DateTime.Now.AddDays(-7) },
                    new Order { CustomerId = 8, Status = "Shipped", OrderDate = DateTime.Now.AddDays(-8) },
                    new Order { CustomerId = 9, Status = "Delivered", OrderDate = DateTime.Now.AddDays(-9) },
                    new Order { CustomerId = 10, Status = "Pending", OrderDate = DateTime.Now.AddDays(-10) }
                };
                context.Orders.AddRange(orders);
                context.SaveChanges(); 
            }
            }
#endif
        }
    }

