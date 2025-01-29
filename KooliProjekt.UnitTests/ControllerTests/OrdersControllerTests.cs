using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
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

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_order_is_missing()
        {
            // Arrange
            int? id = 1;
            var order = (Order)null;

            _orderServiceMock.Setup(x => x.Get(id.Value)).ReturnsAsync(order);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_Should_return_viewresult_when_id_not_null()
        {
            int id = 1;
            Order order = new Order { Id = id };

            _orderServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(order);
            var result = await _controller.Details(id) as ViewResult;

            Assert.NotNull(result);
            Assert.Equal(order, result.Model);

        }

        [Fact]
        public async Task Create_should_return_viewresult()
        {
            // Arrange
            var customers = new List<Customer>
    {
        new Customer { Id = 1, Name = "Customer1" },
        new Customer { Id = 2, Name = "Customer2" }
    };

            // Seame mocki, et GetCustomersAsync tagastaks kliendid
            _orderServiceMock.Setup(x => x.GetCustomersAsync()).ReturnsAsync(customers);

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["Customers"]);
            var viewBagCustomers = result.ViewData["Customers"] as SelectList;
            Assert.NotNull(viewBagCustomers);
            Assert.Equal(customers.Count, viewBagCustomers.Count());
        }


        [Fact]
        public async Task Create_should_return_redirect_to_action_when_model_is_valid()
        {
            // Arrange
            Order order = new Order { Id = 1, CustomerId = 1, Date = System.DateTime.Now, Staatus = "Pending" };

            // Act
            var result = await _controller.Create(order) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_should_return_viewresult_when_model_is_invalid()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerId = 1, Date = DateTime.Now, Staatus = null }; // Staatus on null, et triggerdada vigane ModelState

            // Seame mocki, et GetCustomersAsync tagastaks kliendid
            var customers = new List<Customer>
    {
        new Customer { Id = 1, Name = "Customer1" },
        new Customer { Id = 2, Name = "Customer2" }
    };
            _orderServiceMock.Setup(x => x.GetCustomersAsync()).ReturnsAsync(customers);

            // Seame mocki, et OrderStatuses on olemas
            var orderStatuses = new List<SelectListItem>
    {
        new SelectListItem { Value = "Pending", Text = "Pending" },
        new SelectListItem { Value = "Shipped", Text = "Shipped" },
        new SelectListItem { Value = "Delivered", Text = "Delivered" },
        new SelectListItem { Value = "Cancelled", Text = "Cancelled" }
    };

            // Seame kõik vajalikud ViewBag väärtused
            _controller.ModelState.AddModelError("Staatus", "Staatus on nõutav");

            // Act
            var result = await _controller.Create(order) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ViewData["Customers"]);
            var viewBagCustomers = result.ViewData["Customers"] as SelectList;
            Assert.NotNull(viewBagCustomers);
            Assert.Equal(customers.Count, viewBagCustomers.Count());

            Assert.NotNull(result.ViewData["OrderStatuses"]);
            var viewBagOrderStatuses = result.ViewData["OrderStatuses"] as List<SelectListItem>;
            Assert.NotNull(viewBagOrderStatuses);
            Assert.Equal(orderStatuses.Count, viewBagOrderStatuses.Count);
        }


        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_order_not_found()
        {
            // Arrange
            int id = 1;
            Order order = null;

            _orderServiceMock.Setup(x => x.Get(id)).ReturnsAsync(order);

            // Act
            var result = await _controller.Edit(id, new Order()) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_db_update_concurrency_exception()
        {
            // Arrange
            int id = 1;
            Order order = new Order { Id = id, CustomerId = 1, Date = System.DateTime.Now, Staatus = "Shipped" };
            var exception = new DbUpdateConcurrencyException();

            _orderServiceMock.Setup(x => x.Save(order)).Throws(exception);
            _orderServiceMock.Setup(x => x.Includes(id)).ReturnsAsync(false);

            // Act
            var result = await _controller.Edit(id, order) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_order_is_missing()
        {
            // Arrange
            int id = 1;
            Order order = null;

            _orderServiceMock.Setup(x => x.Get(id)).ReturnsAsync(order);

            // Act
            var result = await _controller.Delete(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index()
        {
            // Arrange
            int id = 1;
            var order = new Order { Id = id, CustomerId = 1, Date = System.DateTime.Now, Staatus = "Shipped" };

            _orderServiceMock.Setup(x => x.Get(id)).ReturnsAsync(order);
            _orderServiceMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteConfirmed(id) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Delete_should_return_viewresult_when_order_is_found()
        {
            // Arrange
            var orderId = 1;
            var order = new Order { Id = orderId };
            _orderServiceMock.Setup(x => x.Get(orderId)).ReturnsAsync(order).Verifiable();

            // Act
            var result = await _controller.Delete(orderId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            Assert.Equal(order, result.Model); 
            _orderServiceMock.Verify(); 
        }

        [Fact]
        public async Task Edit_should_redirect_to_index_if_model_is_valid()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerId = 1, Date = DateTime.Now, Staatus = "Pending" };
            _orderServiceMock.Setup(service => service.Save(It.IsAny<Order>())).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Edit(order.Id, order);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Edit_should_return_notfound_if_concurrency_exception_occurs()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerId = 1, Date = DateTime.Now, Staatus = "Pending" };
            _orderServiceMock.Setup(service => service.Save(It.IsAny<Order>())).ThrowsAsync(new DbUpdateConcurrencyException());

            // Act
            var result = await _controller.Edit(order.Id, order);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }
        [Fact]
        public async Task Edit_should_return_notfound_if_id_mismatch()
        {
            // Arrange
            var order = new Order { Id = 1, CustomerId = 1, Date = DateTime.Now, Staatus = "Pending" };

            // Act
            var result = await _controller.Edit(2, order);  // Siin saame 2, mitte 1

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
        }

    }
}
