using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Tests.Controllers
{
    public class CustomersControllerTests
    {
        private readonly Mock<ICustomerService> _mockService;
        private readonly CustomersController _controller;

        public CustomersControllerTests()
        {
            _mockService = new Mock<ICustomerService>();
            _controller = new CustomersController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithDefaultModel_WhenModelIsNull()
        {
            _mockService.Setup(x => x.List(1, 5, null))
                .ReturnsAsync(new PagedResult<Customer>());

            var result = await _controller.Index(1, null);

            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CustomersIndexModel>(viewResult.Model);
            Assert.NotNull(model.Data);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Details(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenCustomerNotFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Customer)null);

            var result = await _controller.Details(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsView_WhenCustomerExists()
        {
            var customer = new Customer { Id = 1 };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(customer);

            var result = await _controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(customer, viewResult.Model);
        }

        [Fact]
        public void Create_Get_ReturnsView()
        {
            var result = _controller.Create();
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public async Task Create_Post_Redirects_WhenModelIsValid()
        {
            var customer = new Customer { Id = 1, Name = "Test" };

            var result = await _controller.Create(customer);

            _mockService.Verify(x => x.Save(customer), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Create_Post_ReturnsView_WhenModelIsInvalid()
        {
            var customer = new Customer();
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Create(customer);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(customer, view.Model);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Edit(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenCustomerIsNull()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Customer)null);

            var result = await _controller.Edit(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsView_WhenCustomerExists()
        {
            var customer = new Customer { Id = 1 };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(customer);

            var result = await _controller.Edit(1);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(customer, view.Model);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenIdMismatch()
        {
            var customer = new Customer { Id = 2 };

            var result = await _controller.Edit(1, customer);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_Throws_WhenConcurrencyExceptionAndCustomerExists()
        {
            var customer = new Customer { Id = 1 };

            _mockService.Setup(x => x.Save(customer))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(true);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
                _controller.Edit(1, customer));
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenConcurrencyExceptionAndCustomerDoesNotExist()
        {
            var customer = new Customer { Id = 1 };

            _mockService.Setup(x => x.Save(customer))
                .ThrowsAsync(new DbUpdateConcurrencyException());

            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(false);

            var result = await _controller.Edit(1, customer);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_Redirects_WhenSuccessful()
        {
            var customer = new Customer { Id = 1 };

            var result = await _controller.Edit(1, customer);

            _mockService.Verify(x => x.Save(customer), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ReturnsView_WhenModelInvalid()
        {
            var customer = new Customer { Id = 1 };
            _controller.ModelState.AddModelError("Email", "Required");

            var result = await _controller.Edit(1, customer);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(customer, view.Model);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_WhenCustomerNotFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Customer)null);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsView_WhenCustomerExists()
        {
            var customer = new Customer { Id = 1 };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(customer);

            var result = await _controller.Delete(1);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(customer, view.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesAndRedirects_WhenCustomerExists()
        {
            var customer = new Customer { Id = 1 };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(customer);

            var result = await _controller.DeleteConfirmed(1);

            _mockService.Verify(x => x.Delete(1), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_Redirects_WhenCustomerNotFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Customer)null);

            var result = await _controller.DeleteConfirmed(1);

            _mockService.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task CustomerExists_ReturnsTrue_IfExists()
        {
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(true);

            var result = await _controller.CustomerExists(1);

            Assert.True(result);
        }

        [Fact]
        public async Task CustomerExists_ReturnsFalse_IfNotExists()
        {
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(false);

            var result = await _controller.CustomerExists(1);

            Assert.False(result);
        }
    }
}
