using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ControllerTests
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _controller = new CustomersController(_customerServiceMock.Object);
        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Customer>
            {
                new Customer { Id = 1, Name = "Customer1", Email = "customer1@example.com", Phone = 123456789, Address = "Address1" },
                new Customer { Id = 2, Name = "Customer2", Email = "customer2@example.com", Phone = 987654321, Address = "Address2" }
            };

            var pagedResult = new PagedResult<Customer> { Results = data };
            _customerServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;
            // Act
            var result = await _controller.Details(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_customer_is_null()
        {

            // Arrange
            int? id = 1;
            // Act
            _customerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Customer)null).Verifiable();
            var result = await _controller.Details(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _customerServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Details_should_return_viewresult()
        {
            // Arrange
            int? id = 1;
            Customer customer = new Customer() { };
            // Act
            _customerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(customer).Verifiable();
            var result = await _controller.Details(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Create_should_return_resultview()
        {
            // Arrange
            // Act
            var result = _controller.Create();
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public async Task Create_should_return_RedirectToAction_if_modelstate_isvalid()
        {
            // Arrange
            var customer = new Customer() { };
            // Act
            _customerServiceMock.Setup(x => x.Save(It.IsAny<Customer>())).Returns(Task.CompletedTask).Verifiable();
            var result = await _controller.Create(customer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Create_should_return_viewresult()
        {
            // Arrange
            Customer customer = new Customer() { };
            // Act
            _controller.ModelState.AddModelError("key", "error");
            var result = await _controller.Create(customer);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;
            // Act
            var result = await _controller.Edit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task Edit_Should_Return_noutfound_when_customer_is_null()
        {
            // Arrange
            int id = 1;
            // Act
            _customerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Customer)null).Verifiable();
            var result = await _controller.Edit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_viewresult_when_id_and_customer_is_not_null()
        {
            // Arrange
            int id = 1;
            Customer customer = new Customer() { };
            // Act
            _customerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(customer).Verifiable();
            var result = await _controller.Edit(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_not_customer_id()
        {
            // Arrange
            int id = 1;
            Customer customer = new Customer() { Id = 2 };
            // Act
            var result = await _controller.Edit(id, customer);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_should_return_Redirecttoaction()
        {
            // Arrange
            int id = 1;
            Customer customer = new Customer() { Id = id };
            // Act
            _customerServiceMock.Setup(x => x.Save(It.IsAny<Customer>())).Returns(Task.CompletedTask).Verifiable();
            var result = await _controller.Edit(id, customer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_DbUpdateConcureencyException()
        {
            // Arrange
            int id = 1;
            Customer customer = new Customer() { Id = id };
            // Act
            var exception = new DbUpdateConcurrencyException();
            _customerServiceMock.Setup(x => x.Save(It.IsAny<Customer>())).ThrowsAsync(exception).Verifiable();
            _customerServiceMock.Setup(x => x.Includes(It.IsAny<int>())).ReturnsAsync(false).Verifiable();
            var result = await _controller.Edit(id, customer);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_DbUpdateCouncurerencyexception()
        {
            // Arrange
            int id = 1;
            Customer customer = new Customer() { Id = id };

            // Act
            var exception = new DbUpdateConcurrencyException();
            _customerServiceMock.Setup(x => x.Save(It.IsAny<Customer>())).ThrowsAsync(exception).Verifiable();
            _customerServiceMock.Setup(x => x.Includes(It.IsAny<int>())).ReturnsAsync(true).Verifiable();

            // Assert
            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(async () => await _controller.Edit(id, customer));
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Edit_should_return_viewresult_when_modelstate_is_not_valid()
        {
            // Arrange
            int id = 1;
            Customer customer = new Customer() { Id = id };

            // Act
            _controller.ModelState.AddModelError("key", "error");
            var result = await _controller.Edit(id, customer);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);

        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);

        }

        [Fact]
        public async Task Delete_should_return_notfound_when_customer_is_null()
        {
            // Arrange
            int id = 1;

            // Act
            _customerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync((Customer)null).Verifiable();
            var result = await _controller.Delete(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _customerServiceMock.VerifyAll();
        }

        [Fact]
        public async Task Delete_should_return_viewresult()
        {
            // Arrange
            int id = 1;
            Customer customer = new Customer() { Id = id };

            // Act

            _customerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(customer).Verifiable();
            var result = await _controller.Delete(id);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
            _customerServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Deleteconfirmed_should_return_redirecttoaction()
        {
            // Arrange

            int id = 1;
            Customer customer = new Customer() { Id =id };
            // Act
            _customerServiceMock.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(customer).Verifiable();
            var result = await _controller.DeleteConfirmed(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<RedirectToActionResult>(result);
            _customerServiceMock.VerifyAll();

        }

        [Fact]
        public async Task CustomerExists_should_return_bool()
        {
            // Arrange
            int id = 1;

            // Act
            _customerServiceMock.Setup(x => x.Includes(It.IsAny<int>())).ReturnsAsync(It.IsAny<bool>()).Verifiable();
            var result = await (_controller.CustomerExists(id));

            // Assert
            Assert.IsType<bool>(result);
            _customerServiceMock.VerifyAll();
        }
    }
}
