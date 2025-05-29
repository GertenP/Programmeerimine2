namespace KooliProjekt.Data;

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
            new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 },
            new Product { Name = "Banaan", Description = "Looduslik roheline", Price = 1.49m, CategoryId = 1, DiscountPercentage = 5 },
            new Product { Name = "Diivan", Description = "", Price = 299.99m, CategoryId = 2, DiscountPercentage = 20 },
            new Product { Name = "Kohvimasin", Description = "Automaatne espresso", Price = 149.99m, CategoryId = 3, DiscountPercentage = 15 },
            new Product { Name = "T-särk", Description = "Võidusõidustiil", Price = 19.99m, CategoryId = 4, DiscountPercentage = 0 },
            new Product { Name = "Jalgratas", Description = "Mugav ja kiire", Price = 349.99m, CategoryId = 5, DiscountPercentage = 10 },
            new Product { Name = "Arvutihiir", Description = "Ergonoomiline", Price = 29.99m, CategoryId = 6, DiscountPercentage = 5 },
            new Product { Name = "Raamat", Description = "Romaan 2024", Price = 14.99m, CategoryId = 8, DiscountPercentage = 0 },
            new Product { Name = "Shampoo", Description = "Kuse lõhn mees", Price = 9.99m, CategoryId = 9, DiscountPercentage = 20 },
            new Product { Name = "Lastepall", Description = "Pehme ja turvaline", Price = 12.99m, CategoryId = 10, DiscountPercentage = 10 }
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
            new Customer { Name = "Markus Tamm", Email = "markus.tamm@example.com" },
            new Customer { Name = "Anna Kukk", Email = "anna.kukk@example.com"},
            new Customer { Name = "Jaanus Vaher", Email = "jaanus.vaher@example.com"},
            new Customer { Name = "Liis Mänd", Email = "liis.mänd@example.com"},
            new Customer { Name = "Olev Kaar", Email = "olev.kaar@example.com" },
            new Customer { Name = "Kristel Lille", Email = "kristel.lille@example.com" },
            new Customer { Name = "Raul Kivi", Email = "raul.kivi@example.com" },
            new Customer { Name = "Eeva Sõber", Email = "eeva.sõber@example.com" },
            new Customer { Name = "Priit Õun", Email = "priit.õun@example.com" },
            new Customer { Name = "Kertu Mets", Email = "kertu.mets@example.com" }
        };

        // Lisame kliendid andmebaasi
        context.Customers.AddRange(customers);

        // Salvestame muudatused andmebaasi
        context.SaveChanges();
    }
    
}