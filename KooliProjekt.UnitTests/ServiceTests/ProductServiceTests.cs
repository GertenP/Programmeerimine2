using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class ProductServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<IProductsRepository> _repositoryMock;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<IProductsRepository>();
            _productService = new ProductService(_uowMock.Object);

            _uowMock.SetupGet(u => u.ProductsRepository)
                    .Returns(_repositoryMock.Object);
        }
        
        [Fact]
        public async Task List_should_return_paged_products()
        {
            // Arrange
            var products = new List<Product> { new Product { Id = 1 }, new Product { Id = 2 } };
            var pagedResult = new PagedResult<Product> { Results = products };
            _repositoryMock.Setup(r => r.List(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(pagedResult);

            // Act
            var result = await _productService.List(1, 10);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Get_should_return_product()
        {
            // Arrange
            var product = new Product { Id = 1 };
            _repositoryMock.Setup(r => r.Get(1)).ReturnsAsync(product);

            // Act
            var result = await _productService.Get(1);

            // Assert
            Assert.Equal(product, result);
        }

        [Fact]
        public async Task Save_should_call_repository_save()
        {
            // Arrange
            var product = new Product { Id = 1 };

            // Act
            await _productService.Save(product);

            // Assert
            _repositoryMock.Verify(r => r.Save(product), Times.Once);
        }

        [Fact]
        public async Task Delete_should_call_repository_delete()
        {
            // Arrange
            var id = 1;

            // Act
            await _productService.Delete(id);

            // Assert
            _repositoryMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public async Task Includes_should_return_true_if_product_exists()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Includes(1)).ReturnsAsync(true);

            // Act
            var result = await _productService.Includes(1);

            // Assert
            Assert.True(result);
        }
    }
}
