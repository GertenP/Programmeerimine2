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
    public class CategoriesControllerTests
    {

        private readonly Mock<ICategoryItemService> _categoryItemServiceMock;
        private readonly CategoriesController _controller;
        public CategoriesControllerTests()
        {

            _categoryItemServiceMock = new Mock<ICategoryItemService>();
            _controller = new CategoriesController(_categoryItemServiceMock.Object);

        }

        [Fact]
        public async Task Index_should_return_correct_view_with_data()
        {
            // Arrange
            int page = 1;
            var data = new List<Category>
            {
                new Category { Id = 1, Name = "Kategooria1" },
                new Category { Id = 2, Name = "Kategooria2" }
            };

            var pagedResult = new PagedResult<Category> {Results = data};
            _categoryItemServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);

        }
    }
}
