using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.Rendering;
using Moq;
using Xunit;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Tests.Controllers
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockService;
        private readonly ProductsController _controller;

        public ProductsControllerTests()
        {
            _mockService = new Mock<IProductService>();
            _controller = new ProductsController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithModelAndCategories()
        {
            _mockService.Setup(x => x.List(1, 5, null)).ReturnsAsync(new PagedResult<Product>());
            _mockService.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(new List<Category>());

            var result = await _controller.Index(1, null);

            var view = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ProductsIndexModel>(view.Model);
            Assert.NotNull(view.ViewData["Categories"]);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_IfIdNull()
        {
            var result = await _controller.Details(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_IfProductNotFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Product)null);

            var result = await _controller.Details(1);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsView_IfProductExists()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(new Product { Id = 1 });
            _mockService.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(new List<Category>());

            var result = await _controller.Details(1);

            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(view.Model);
        }

        [Fact]
        public async Task Create_Get_ReturnsViewWithCategories()
        {
            _mockService.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(new List<Category>());

            var result = await _controller.Create();

            var view = Assert.IsType<ViewResult>(result);
            Assert.NotNull(_controller.ViewBag.Categories);
        }

        [Fact]
        public async Task Create_Post_Redirects_WhenModelIsValid()
        {
            var product = new Product { Id = 1 };

            var result = await _controller.Create(product);

            _mockService.Verify(x => x.Save(product), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Create_Post_ReturnsView_WhenModelInvalid()
        {
            var product = new Product();
            _controller.ModelState.AddModelError("Name", "Required");
            _mockService.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(new List<Category>());

            var result = await _controller.Create(product);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(product, view.Model);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenIdNull()
        {
            var result = await _controller.Edit(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenProductNull()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Product)null);

            var result = await _controller.Edit(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsView_WhenProductFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(new Product { Id = 1 });
            _mockService.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(new List<Category>());

            var result = await _controller.Edit(1);

            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(view.Model);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_IfIdMismatch()
        {
            var product = new Product { Id = 2 };
            var result = await _controller.Edit(1, product);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_ThrowsConcurrency_WhenProductStillExists()
        {
            var product = new Product { Id = 1 };
            _mockService.Setup(x => x.Save(product)).ThrowsAsync(new DbUpdateConcurrencyException());
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(true);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
                _controller.Edit(1, product));
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenProductNoLongerExists()
        {
            var product = new Product { Id = 1 };
            _mockService.Setup(x => x.Save(product)).ThrowsAsync(new DbUpdateConcurrencyException());
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(false);

            var result = await _controller.Edit(1, product);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_Redirects_WhenSuccess()
        {
            var product = new Product { Id = 1 };

            var result = await _controller.Edit(1, product);

            _mockService.Verify(x => x.Save(product), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ReturnsView_WhenModelInvalid()
        {
            var product = new Product { Id = 1 };
            _controller.ModelState.AddModelError("Price", "Required");
            _mockService.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(new List<Category>());

            var result = await _controller.Edit(1, product);

            var view = Assert.IsType<ViewResult>(result);
            Assert.Equal(product, view.Model);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_IfIdNull()
        {
            var result = await _controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_IfProductNull()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Product)null);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsView_IfProductFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(new Product { Id = 1 });

            var result = await _controller.Delete(1);

            var view = Assert.IsType<ViewResult>(result);
            Assert.IsType<Product>(view.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_CallsDeleteAndRedirects()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(new Product { Id = 1 });

            var result = await _controller.DeleteConfirmed(1);

            _mockService.Verify(x => x.Delete(1), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_SkipsDelete_IfProductNotFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Product)null);

            var result = await _controller.DeleteConfirmed(1);

            _mockService.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task ProductExists_ReturnsTrue_IfExists()
        {
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(true);
            var result = await _controller.ProductExists(1);
            Assert.True(result);
        }

        [Fact]
        public async Task ProductExists_ReturnsFalse_IfNotExists()
        {
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(false);
            var result = await _controller.ProductExists(1);
            Assert.False(result);
        }
    }
}
