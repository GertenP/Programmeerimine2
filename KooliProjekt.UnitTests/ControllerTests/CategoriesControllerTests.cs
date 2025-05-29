using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using Moq;
using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Tests.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryItemService> _mockService;
        private readonly CategoriesController _controller;

        public CategoriesControllerTests()
        {
            _mockService = new Mock<ICategoryItemService>();
            _controller = new CategoriesController(_mockService.Object);
        }

        [Fact]
        public async Task Index_ReturnsViewWithDefaultModel_WhenModelIsNull()
        {
            // Arrange
            _mockService
                .Setup(x => x.List(1, 5, null))
                .ReturnsAsync(new PagedResult<Category>());

            // Act
            var result = await _controller.Index(1, null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CategoriesIndexModel>(viewResult.Model);
            Assert.NotNull(model.Data);
        }



        [Fact]
        public async Task Details_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Details(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsNotFound_WhenCategoryNotFound()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Category)null);

            var result = await _controller.Details(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Details_ReturnsView_WhenCategoryExists()
        {
            var category = new Category { Id = 1, Name = "Test" };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(category);

            var result = await _controller.Details(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(category, viewResult.Model);
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
            var category = new Category { Id = 1, Name = "Test" };

            var result = await _controller.Create(category);

            _mockService.Verify(x => x.Save(category), Times.Once);
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectResult.ActionName);
        }

        [Fact]
        public async Task Create_Post_ReturnsView_WhenModelIsInvalid()
        {
            var category = new Category { Id = 1, Name = "Test" };
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Create(category);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(category, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Edit(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsNotFound_WhenCategoryIsNull()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Category)null);

            var result = await _controller.Edit(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Get_ReturnsView_WhenCategoryExists()
        {
            var category = new Category { Id = 1, Name = "Test" };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(category);

            var result = await _controller.Edit(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(category, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenIdMismatch()
        {
            var category = new Category { Id = 2, Name = "Mismatch" };

            var result = await _controller.Edit(1, category);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Edit_Post_Redirects_WhenModelIsValid()
        {
            var category = new Category { Id = 1, Name = "Test" };
            _mockService.Setup(x => x.Save(category)).Returns(Task.CompletedTask);

            var result = await _controller.Edit(1, category);

            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ReturnsView_WhenModelInvalid()
        {
            var category = new Category { Id = 1, Name = "Test" };
            _controller.ModelState.AddModelError("Name", "Required");

            var result = await _controller.Edit(1, category);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(category, viewResult.Model);
        }

        [Fact]
        public async Task Edit_Post_Throws_WhenConcurrencyAndExists()
        {
            var category = new Category { Id = 1, Name = "Test" };

            _mockService.Setup(x => x.Save(category)).ThrowsAsync(new DbUpdateConcurrencyException());
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(true);

            await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() =>
                _controller.Edit(1, category));
        }

        [Fact]
        public async Task Edit_Post_ReturnsNotFound_WhenConcurrencyAndNotExists()
        {
            var category = new Category { Id = 1, Name = "Test" };

            _mockService.Setup(x => x.Save(category)).ThrowsAsync(new DbUpdateConcurrencyException());
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(false);

            var result = await _controller.Edit(1, category);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_WhenIdIsNull()
        {
            var result = await _controller.Delete(null);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsNotFound_WhenCategoryIsNull()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Category)null);

            var result = await _controller.Delete(1);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Get_ReturnsView_WhenCategoryExists()
        {
            var category = new Category { Id = 1, Name = "Test" };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(category);

            var result = await _controller.Delete(1);

            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(category, viewResult.Model);
        }

        [Fact]
        public async Task DeleteConfirmed_DeletesAndRedirects_WhenCategoryExists()
        {
            var category = new Category { Id = 1, Name = "Test" };
            _mockService.Setup(x => x.Get(1)).ReturnsAsync(category);

            var result = await _controller.DeleteConfirmed(1);

            _mockService.Verify(x => x.Delete(1), Times.Once);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task DeleteConfirmed_Redirects_WhenCategoryIsNull()
        {
            _mockService.Setup(x => x.Get(1)).ReturnsAsync((Category)null);

            var result = await _controller.DeleteConfirmed(1);

            _mockService.Verify(x => x.Delete(It.IsAny<int>()), Times.Never);
            var redirect = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirect.ActionName);
        }

        [Fact]
        public async Task CategoryExists_ReturnsTrueOrFalse()
        {
            _mockService.Setup(x => x.Includes(1)).ReturnsAsync(true);
            var exists = await _controller.CategoryExists(1);
            Assert.True(exists);

            _mockService.Setup(x => x.Includes(2)).ReturnsAsync(false);
            var notExists = await _controller.CategoryExists(2);
            Assert.False(notExists);
        }
    }
}
