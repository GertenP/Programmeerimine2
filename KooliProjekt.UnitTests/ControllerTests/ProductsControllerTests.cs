using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _productServiceMock;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductsController(_productServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Product>
         {
        new Product { Id = 1, Name = "Nimi1", Description = "Description1", Price = 12, CategoryId = 1, Discount = 50 },
        new Product { Id = 2, Name = "Nimi2", Description = "Description2", Price = 42, CategoryId = 2, Discount = 0 }
        };
            var pagedResult = new PagedResult<Product> { Results = data };
            _productServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            var categories = new List<Category>
          {
            new Category { Id = 1, Name = "Category1" },
          new Category { Id = 2, Name = "Category2" }
            };
            _productServiceMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);


        }

    }
}
