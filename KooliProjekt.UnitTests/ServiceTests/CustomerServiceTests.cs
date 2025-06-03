using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CustomerServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Get_should_return_customer_by_id()
        {
            // Arrange
            var customer = new Customer { Name = "Mari", Email = "mari@example.com" };
            DbContext.Customers.Add(customer);
            DbContext.SaveChanges();

            var service = new CustomerService(DbContext);

            // Act
            var result = await service.Get(customer.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Mari", result.Name);
        }

        [Fact]
        public async Task Get_should_return_null_if_customer_not_found()
        {
            // Arrange
            var service = new CustomerService(DbContext);

            // Act
            var result = await service.Get(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Includes_should_return_true_if_customer_exists()
        {
            // Arrange
            var customer = new Customer { Name = "Peeter", Email = "peeter@example.com" };
            DbContext.Customers.Add(customer);
            DbContext.SaveChanges();

            var service = new CustomerService(DbContext);

            // Act
            var result = await service.Includes(customer.Id);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Includes_should_return_false_if_customer_not_exists()
        {
            // Arrange
            var service = new CustomerService(DbContext);

            // Act
            var result = await service.Includes(999);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_should_remove_customer_if_exists()
        {
            // Arrange
            var customer = new Customer { Name = "Kati", Email = "kati@example.com" };
            DbContext.Customers.Add(customer);
            DbContext.SaveChanges();

            var service = new CustomerService(DbContext);

            // Act
            await service.Delete(customer.Id);

            // Assert
            Assert.Empty(DbContext.Customers);
        }

        [Fact]
        public async Task Delete_should_do_nothing_if_customer_not_found()
        {
            // Arrange
            var service = new CustomerService(DbContext);

            // Act
            await service.Delete(999);

            // Assert
            Assert.Empty(DbContext.Customers);
        }

        [Fact]
        public async Task Save_should_add_new_customer_if_id_is_zero()
        {
            // Arrange
            var customer = new Customer { Name = "Karl", Email = "karl@example.com" };
            var service = new CustomerService(DbContext);

            // Act
            await service.Save(customer);

            // Assert
            Assert.Single(DbContext.Customers);
            Assert.Equal("Karl", DbContext.Customers.First().Name);
        }

        [Fact]
        public async Task Save_should_update_existing_customer()
        {
            // Arrange
            var customer = new Customer { Name = "Liis", Email = "liis@example.com" };
            DbContext.Customers.Add(customer);
            DbContext.SaveChanges();

            customer.Name = "Liis Updated";
            var service = new CustomerService(DbContext);

            // Act
            await service.Save(customer);

            // Assert
            Assert.Equal(1, DbContext.Customers.Count());
            Assert.Equal("Liis Updated", DbContext.Customers.First().Name);
        }

        [Fact]
        public async Task List_should_return_all_customers_without_filter()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                DbContext.Customers.Add(new Customer { Name = $"Customer{i}", Email = $"c{i}@example.com" });
            }
            DbContext.SaveChanges();

            var service = new CustomerService(DbContext);

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
            DbContext.Customers.Add(new Customer { Name = "Alice", Email = "alice@example.com" });
            DbContext.Customers.Add(new Customer { Name = "Bob", Email = "bob@example.com" });
            DbContext.SaveChanges();

            var search = new CustomerSearch { Keyword = "Ali" };
            var service = new CustomerService(DbContext);

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Single(result.Results);
            Assert.Equal("Alice", result.Results.First().Name);
        }
    }
}
