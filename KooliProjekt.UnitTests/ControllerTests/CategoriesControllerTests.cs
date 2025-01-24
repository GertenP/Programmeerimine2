using KooliProjekt.Controllers;
using KooliProjekt.Data;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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

            var pagedResult = new PagedResult<Category> { Results = data };
            _categoryItemServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);

        }
        [Fact]
        public async Task Details_should_return_notfound_when_id_is_missing()
        {
            // Arrange
            int? id = null;

            //Act

            var result = await _controller.Details(id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_list_is_missing_with_id()
        {
            int? id = 1;
            var list = (Category)null;

            _categoryItemServiceMock.Setup(x => x.Get(id.Value)).ReturnsAsync(list);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task Create_should_return_viewresult()
        {
            //Arrange
            //Act
            var result = _controller.Create() as ViewResult;
            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Create_should_return_RedirectToAction_when_modalstate_is_valid()
        {
            //Arrange
            Category category = new Category() { Id = 1, Name = "Kategooria1" };

            // Act
            var result = await _controller.Create(category) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

        }

        [Fact]
        public async Task Create_should_return_viewresult_when_modelstate_is_not_valid()
        {
            // Arrange
            var category = new Category() { Id = 1, Name = null }; // Mudel, millel Name on null

            // Lisame ModelState vigade simuleerimiseks viga
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(category) as ViewResult;

            // Assert
            Assert.NotNull(result); // Kontrollime, et tagastatakse ViewResult
            Assert.Equal(category, result?.Model); // Veendume, et mudel on õige
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_is_null()
        {
            //Arrange
            int? id = null;

            //Act
            var result = await _controller.Edit(id) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_modalstate_isvalid_and_DbUpdateConcurrencyException()
        {
            // Arrange
            int id = 1;
            var categories = new List<Category>()
            {
                new Category() {Id = 2, Name ="Name"},
                new Category() {Id = 3, Name ="Name2"},
            };
            Category category = new Category()
            {
                Id = id,
                Name = "Name"
            };
            var exception = new DbUpdateConcurrencyException();

            // Act
            _categoryItemServiceMock.Setup(x => x.Save(category)).Throws(exception);
            _categoryItemServiceMock.Setup(x => x.Includes(id)).ReturnsAsync(false);

            var result = await _controller.Edit(id, category) as NotFoundResult;
            //Assert
            Assert.NotNull(result);



        }

        [Fact]
        public async Task Edit_should_return_NotFound_when_id_is_not_equa_category_id()
        {
            //Arrange
            var category = new Category() { Id = 1, Name = "Puuvili" };
            int id = 2;

            //Act
            var result = await _controller.Edit(id, category) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
        }
    }
}
