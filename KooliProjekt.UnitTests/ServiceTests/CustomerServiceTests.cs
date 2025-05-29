using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CustomerServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ICustomersRepository> _repositoryMock;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<ICustomersRepository>();
            _customerService = new CustomerService(_uowMock.Object);

            _uowMock.SetupGet(r => r.CustomersRepository)
                    .Returns(_repositoryMock.Object);
        }

        [Fact]
        public async Task Get_should_return_customer()
        {
            // Arrange
            var customer = new Customer { Id = 1 };
            _repositoryMock.Setup(r => r.Get(1)).ReturnsAsync(customer);

            // Act
            var result = await _customerService.Get(1);

            // Assert
            Assert.Equal(customer, result);
        }

        [Fact]
        public async Task Delete_should_call_repository_delete()
        {
            // Arrange
            var customerId = 1;

            // Act
            await _customerService.Delete(customerId);

            // Assert
            _repositoryMock.Verify(r => r.Delete(customerId), Times.Once);
        }

        [Fact]
        public async Task Includes_should_return_true_if_customer_exists()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Includes(1)).ReturnsAsync(true);

            // Act
            var result = await _customerService.Includes(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task List_should_return_paged_customers()
        {
            // Arrange
            var results = new List<Customer>
            {
                new Customer { Id = 1 },
                new Customer { Id = 2 }
            };
            var pagedResult = new PagedResult<Customer> { Results = results };
            _repositoryMock.Setup(r => r.List(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(pagedResult);

            // Act
            var result = await _customerService.List(1, 10);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Save_should_call_repository_save()
        {
            // Arrange
            var customer = new Customer { Id = 1 };

            // Act
            await _customerService.Save(customer);

            // Assert
            _repositoryMock.Verify(r => r.Save(customer), Times.Once);
        }
    }
}
