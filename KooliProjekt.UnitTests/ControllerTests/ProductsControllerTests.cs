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
        public async Task Index_ShouldReturnCorrectViewWithDataAndViewBag()
        {
            // Arrange
            int page = 1;
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };
            var products = new List<Product>
            {
                new Product { Id = 1, Name = "Phone", CategoryId = 1 },
                new Product { Id = 2, Name = "Laptop", CategoryId = 1 }
            };
            var pagedResult = new PagedResult<Product> { Results = products };

            _productServiceMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(categories);
            _productServiceMock.Setup(x => x.List(page, It.IsAny<int>())).ReturnsAsync(pagedResult);

            // Act
            var result = await _controller.Index(page) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(pagedResult, result.Model);

            Assert.True(result.ViewData.ContainsKey("Categories"));
            var viewBagCategories = result.ViewData["Categories"] as List<Category>;
            Assert.NotNull(viewBagCategories);
            Assert.Equal(categories, viewBagCategories);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_ShouldReturnNotFound_WhenProductIsMissing()
        {
            // Arrange
            int? id = 1;
            Product product = null;

            _productServiceMock.Setup(x => x.Get(id.Value)).ReturnsAsync(product);

            // Act
            var result = await _controller.Details(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Details_ShouldReturnViewResult_WhenProductIsFound()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Phone", CategoryId = 1 };
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };

            _productServiceMock.Setup(x => x.Get(id)).ReturnsAsync(product);
            _productServiceMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.Details(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product, result.Model);

            Assert.True(result.ViewData.ContainsKey("Categories"));
            var viewBagCategories = result.ViewData["Categories"] as List<Category>;
            Assert.NotNull(viewBagCategories);
            Assert.Equal(categories, viewBagCategories);
        }

        [Fact]
        public async Task Create_Get_ShouldReturnViewResultWithCategoriesInViewBag()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };

            _productServiceMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.Create() as ViewResult;

            // Assert
            Assert.NotNull(result);

            Assert.True(result.ViewData.ContainsKey("Categories"));
            var viewBagCategories = result.ViewData["Categories"] as SelectList;
            Assert.NotNull(viewBagCategories);
            Assert.Equal(categories.Count, viewBagCategories.Count());
        }

        [Fact]
        public async Task Create_Post_ShouldRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Phone", CategoryId = 1 };

            // Act
            var result = await _controller.Create(product) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Create_Post_ShouldReturnViewResult_WhenModelIsInvalid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = null, CategoryId = 1 }; // Invalid model
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };

            _productServiceMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(categories);
            _controller.ModelState.AddModelError("Name", "Name is required");

            // Act
            var result = await _controller.Create(product) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product, result.Model);

            Assert.True(result.ViewData.ContainsKey("Categories"));
            var viewBagCategories = result.ViewData["Categories"] as SelectList;
            Assert.NotNull(viewBagCategories);
            Assert.Equal(categories.Count, viewBagCategories.Count());
        }

        [Fact]
        public async Task Edit_Get_ShouldReturnNotFound_WhenIdIsNull()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Get_ShouldReturnNotFound_WhenProductIsMissing()
        {
            // Arrange
            int id = 1;
            Product product = null;

            _productServiceMock.Setup(x => x.Get(id)).ReturnsAsync(product);

            // Act
            var result = await _controller.Edit(id) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_Get_ShouldReturnViewResult_WhenProductIsFound()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Phone", CategoryId = 1 };
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Electronics" },
                new Category { Id = 2, Name = "Books" }
            };

            _productServiceMock.Setup(x => x.Get(id)).ReturnsAsync(product);
            _productServiceMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(categories);

            // Act
            var result = await _controller.Edit(id) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product, result.Model);

            Assert.True(result.ViewData.ContainsKey("Categories"));
            var viewBagCategories = result.ViewData["Categories"] as SelectList;
            Assert.NotNull(viewBagCategories);
            Assert.Equal(categories.Count, viewBagCategories.Count());
        }

        [Fact]
        public async Task Edit_Post_ShouldRedirectToIndex_WhenModelIsValid()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Phone", CategoryId = 1 };

            // Act
            var result = await _controller.Edit(product.Id, product) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
        }

        [Fact]
        public async Task Edit_Post_ShouldReturnNotFound_WhenIdMismatch()
        {
            // Arrange
            var product = new Product { Id = 1, Name = "Phone", CategoryId = 1 };

            // Act
            var result = await _controller.Edit(2, product) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_DbUpdateConcurrencyException()
        {
            int id = 1;
            var product = new Product { Id = id };
            var exception = new DbUpdateConcurrencyException();
            // Act
            _productServiceMock.Setup(x => x.Save(It.IsAny<Product>())).ThrowsAsync(exception).Verifiable();
            _productServiceMock.Setup(x => x.Includes(It.IsAny<int>())).ReturnsAsync(false).Verifiable();
            var result = await _controller.Edit(id, product);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
            _productServiceMock.VerifyAll();

        }

        [Fact]
        public async Task Edit_should_return_DbUpdateConcurrencyException()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id };
            var exception = new DbUpdateConcurrencyException();

            _productServiceMock.Setup(x => x.Save(It.IsAny<Product>())).ThrowsAsync(exception).Verifiable();

            _productServiceMock.Setup(x => x.Includes(It.IsAny<int>())).ReturnsAsync(true).Verifiable();

            // Act & Assert
            var result = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => _controller.Edit(id, product));

            Assert.Equal(exception.GetType(), result.GetType());

            _productServiceMock.Verify(x => x.Save(It.IsAny<Product>()), Times.Once);
            _productServiceMock.Verify(x => x.Includes(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Edit_should_return_view_with_product_when_model_state_invalid()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Test Product", Description = "Test Description", Price = 10.0m, CategoryId = 1, Discount = 5 };

            var mockCategories = new List<Category>
    {
        new Category { Id = 1, Name = "Category1" },
        new Category { Id = 2, Name = "Category2" }
    };

            _productServiceMock.Setup(x => x.GetCategoriesAsync()).ReturnsAsync(mockCategories).Verifiable(); // Mock GetCategories or similar method

            _controller.ModelState.AddModelError("Name", "The Name field is required.");

            // Act
            var result = await _controller.Edit(id, product);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<Product>(viewResult.Model);
            Assert.Equal(product, model);  
            _productServiceMock.Verify(x => x.GetCategoriesAsync(), Times.Once);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Arrange
            int? id = null;

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result);  
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_product_not_found()
        {
            // Arrange
            int id = 1;
            _productServiceMock.Setup(x => x.Get(id)).ReturnsAsync((Product)null); 

            // Act
            var result = await _controller.Delete(id);

            // Assert
            Assert.IsType<NotFoundResult>(result); 
        }

        [Fact]
        public async Task Delete_should_return_view_when_product_found()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Test Product" };
            _productServiceMock.Setup(x => x.Get(id)).ReturnsAsync(product);  

            // Act
            var result = await _controller.Delete(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result); 
            var model = Assert.IsAssignableFrom<Product>(viewResult.Model); 
            Assert.Equal(product, model); 
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect_to_index_when_product_found_and_deleted()
        {
            // Arrange
            int id = 1;
            var product = new Product { Id = id, Name = "Test Product" };
            _productServiceMock.Setup(x => x.Get(id)).ReturnsAsync(product);  
            _productServiceMock.Setup(x => x.Delete(id)).Returns(Task.CompletedTask);  

            // Act
            var result = await _controller.DeleteConfirmed(id);

            // Assert
            var redirectResult = Assert.IsType<RedirectToActionResult>(result);  
            Assert.Equal("Index", redirectResult.ActionName);  
            _productServiceMock.Verify(x => x.Delete(id), Times.Once);  
        }

        [Fact]
        public async Task ProductExists_should_return_true_when_product_exists()
        {
            // Arrange
            int id = 1;
            _productServiceMock.Setup(x => x.Includes(It.IsAny<int>())).ReturnsAsync(true).Verifiable();

            // Act
            var result = await _controller.ProductExists(id);

            // Assert
            Assert.NotNull(result); 
            Assert.IsType<bool>(result);
            _productServiceMock.VerifyAll(); 
        }

    }
}
