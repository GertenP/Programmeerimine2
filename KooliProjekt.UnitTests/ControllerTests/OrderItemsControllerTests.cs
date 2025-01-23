using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class OrderItemsControllerTests
    {
        private readonly Mock<IOrderItemService> _orderItemServiceMock;
        private readonly OrderItemsController _controller;

        public OrderItemsControllerTests()
        {
            _orderItemServiceMock = new Mock<IOrderItemService>();
            _controller = new OrderItemsController(_orderItemServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data_and_viewbag()
        {
            // Arrange
            int page = 1;

            // Mockitud tellimuse andmed
            var orderItems = new List<OrderItem>
            {
                new OrderItem { Id = 1, OrderId = 1, ProductId = 1, Quantity = 2, Price = 20, Discount = 5 },
                new OrderItem { Id = 2, OrderId = 2, ProductId = 2, Quantity = 3, Price = 30, Discount = 10 }
            };
            var pagedResult = new PagedResult<OrderItem> { Results = orderItems };
            _orderItemServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Mockitud tooted
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Product1" },
                new Product { Id = 2, Name = "Product2" }
            };
            _orderItemServiceMock.Setup(x => x.GetProductsAsync()).ReturnsAsync(products);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);

            // Kontrollime, kas ViewBag.Products on määratud ja sisaldab õigeid andmeid
            Assert.True(result.ViewData.ContainsKey("Products"));
            var viewBagProducts = result.ViewData["Products"] as List<Product>;
            Assert.NotNull(viewBagProducts);
            Assert.Equal(products, viewBagProducts);
        }
    }
}
