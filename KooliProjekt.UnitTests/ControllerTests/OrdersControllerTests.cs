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
    public class OrdersControllerTests
    {
        private readonly Mock<IOrderService> _orderServiceMock;
        private readonly OrdersController _controller;

        public OrdersControllerTests()
        {
            _orderServiceMock = new Mock<IOrderService>();
            _controller = new OrdersController(_orderServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data_and_viewbag()
        {
            // Arrange
            int page = 1;

            // Mockitud tellimuste andmed
            var orders = new List<Order>
            {
                new Order { Id = 1, CustomerId = 1, Date = System.DateTime.Now, Staatus = "Pending" },
                new Order { Id = 2, CustomerId = 2, Date = System.DateTime.Now, Staatus = "Shipped" }
            };
            var pagedResult = new PagedResult<Order> { Results = orders };
            _orderServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Mockitud kliendid
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Customer1" },
                new Customer { Id = 2, Name = "Customer2" }
            };
            _orderServiceMock.Setup(x => x.GetCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);

            // Kontrollime, kas ViewBag.Customers on määratud ja sisaldab õigeid andmeid
            Assert.True(result.ViewData.ContainsKey("Customers"));
            var viewBagCustomers = result.ViewData["Customers"] as List<Customer>;
            Assert.NotNull(viewBagCustomers);
            Assert.Equal(customers, viewBagCustomers);
        }
    }
}
