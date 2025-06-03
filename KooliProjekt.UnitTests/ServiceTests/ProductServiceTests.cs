using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ProductServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task GetCategoriesAsync_should_return_all_categories()
        {
            // Arrange
            DbContext.Categories.Add(new Category { Name = "Cat1" });
            DbContext.Categories.Add(new Category { Name = "Cat2" });
            DbContext.SaveChanges();

            var service = new ProductService(DbContext);

            // Act
            var result = await service.GetCategoriesAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task GetProductsAsync_should_return_all_products()
        {
            // Arrange
            DbContext.Products.Add(new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 });
            DbContext.Products.Add(new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 });
            DbContext.SaveChanges();

            var service = new ProductService(DbContext);

            // Act
            var result = await service.GetProductsAsync();

            // Assert
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task List_should_return_paged_result_without_search()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                DbContext.Products.Add(new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 });
            }
            DbContext.SaveChanges();

            var service = new ProductService(DbContext);

            // Act
            var result = await service.List(1, 5);

            // Assert
            Assert.Equal(5, result.Results.Count);
            Assert.Equal(10, result.RowCount);
        }

        [Fact]
        public async Task List_should_filter_by_keyword()
        {
            // Arrange
            DbContext.Products.Add(new Product { Name = "Apple", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 });
            DbContext.Products.Add(new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 });
            DbContext.SaveChanges();

            var service = new ProductService(DbContext);
            var search = new ProductSearch { Keyword = "Apple" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Single(result.Results);
            Assert.Equal("Apple", result.Results.First().Name);
        }

        [Fact]
        public async Task Get_should_return_product_by_id()
        {
            // Arrange
            var product = new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 };
            DbContext.Products.Add(product);
            DbContext.SaveChanges();

            var service = new ProductService(DbContext);

            // Act
            var result = await service.Get(product.Id);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Get_should_return_null_if_not_found()
        {
            // Arrange
            var service = new ProductService(DbContext);

            // Act
            var result = await service.Get(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Save_should_add_new_product_if_id_is_zero()
        {
            // Arrange
            var product = new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 };
            var service = new ProductService(DbContext);

            // Act
            await service.Save(product);

            // Assert
            Assert.Equal(1, DbContext.Products.Count());
            Assert.Equal("Õun", DbContext.Products.First().Name);
        }

        [Fact]
        public async Task Save_should_update_existing_product()
        {
            // Arrange
            var product = new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 };
            DbContext.Products.Add(product);
            DbContext.SaveChanges();

            product.Name = "Updated";
            var service = new ProductService(DbContext);

            // Act
            await service.Save(product);

            // Assert
            Assert.Equal(1, DbContext.Products.Count());
            Assert.Equal("Updated", DbContext.Products.First().Name);
        }

        [Fact]
        public async Task Delete_should_remove_product_if_exists()
        {
            // Arrange
            var product = new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10 };
            DbContext.Products.Add(product);
            DbContext.SaveChanges();

            var service = new ProductService(DbContext);

            // Act
            await service.Delete(product.Id);

            // Assert
            Assert.Empty(DbContext.Products);
        }

        [Fact]
        public async Task Delete_should_do_nothing_if_product_not_found()
        {
            // Arrange
            var service = new ProductService(DbContext);

            // Act
            await service.Delete(999); // ID does not exist

            // Assert
            Assert.Empty(DbContext.Products);
        }

        [Fact]
        public async Task Includes_should_return_true_if_product_exists()
        {
            // Arrange
            var product = new Product { Name = "Õun", Description = "Magus punane", Price = 1.99m, CategoryId = 1, DiscountPercentage = 10};
            DbContext.Products.Add(product);
            DbContext.SaveChanges();

            var service = new ProductService(DbContext);

            // Act
            var result = await service.Includes(product.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Includes_should_return_false_if_product_not_exists()
        {
            // Arrange
            var service = new ProductService(DbContext);

            // Act
            var result = await service.Includes(999);

            // Assert
            Assert.False(result);
        }
    }
}
