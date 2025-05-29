using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CategoryItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _uowMock;
        private readonly Mock<ICategoryRepository> _repositoryMock;
        private readonly CategoryItemService _categoryItemService;

        public CategoryItemServiceTests()
        {
            _uowMock = new Mock<IUnitOfWork>();
            _repositoryMock = new Mock<ICategoryRepository>();
            _categoryItemService = new CategoryItemService(_uowMock.Object);

            _uowMock.SetupGet(u => u.CategoryRepository)
                    .Returns(_repositoryMock.Object);
        }

        [Fact]
        public async Task Delete_should_call_repository_delete()
        {
            // Arrange
            var id = 1;

            // Act
            await _categoryItemService.Delete(id);

            // Assert
            _repositoryMock.Verify(r => r.Delete(id), Times.Once);
        }

        [Fact]
        public async Task Get_with_id_should_return_category()
        {
            // Arrange
            var category = new Category { Id = 1 };
            _repositoryMock.Setup(r => r.Get(1)).ReturnsAsync(category);

            // Act
            var result = await _categoryItemService.Get(1);

            // Assert
            Assert.Equal(category, result);
        }

        [Fact]
        public async Task Get_with_nullable_id_should_return_category()
        {
            // Arrange
            var category = new Category { Id = 2 };
            _repositoryMock.Setup(r => r.Get((int?)2)).ReturnsAsync(category);

            // Act
            var result = await _categoryItemService.Get((int?)2);

            // Assert
            Assert.Equal(category, result);
        }

        [Fact]
        public async Task Includes_should_return_true_if_category_exists()
        {
            // Arrange
            _repositoryMock.Setup(r => r.Includes(1)).ReturnsAsync(true);

            // Act
            var result = await _categoryItemService.Includes(1);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task List_should_return_paged_categories()
        {
            // Arrange
            var categories = new List<Category> 
            { 
                new Category { Id = 1 }, 
                new Category { Id = 2 } 
            };
            var pagedResult = new PagedResult<Category> { Results = categories };
            _repositoryMock.Setup(r => r.List(It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync(pagedResult);

            // Act
            var result = await _categoryItemService.List(1, 10);

            // Assert
            Assert.Equal(pagedResult, result);
        }

        [Fact]
        public async Task Save_should_call_repository_save()
        {
            // Arrange
            var category = new Category { Id = 3 };

            // Act
            await _categoryItemService.Save(category);

            // Assert
            _repositoryMock.Verify(r => r.Save(category), Times.Once);
        }
    }
}
