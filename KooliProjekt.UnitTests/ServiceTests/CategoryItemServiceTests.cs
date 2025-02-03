using KooliProjekt.Data;
using KooliProjekt.Data.Repositories;
using KooliProjekt.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CategoryItemServiceTests
    {
        private readonly Mock<IUnitOfWork> _uof;
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly ICategoryItemService _service;

        public CategoryItemServiceTests()
        {
            _uof = new Mock<IUnitOfWork>();
            _categoryRepository = new Mock<ICategoryRepository>();
            _service = new CategoryItemService(_uof.Object);
            _uof.SetupGet(r => r.CategoryRepository).Returns(_categoryRepository.Object);
        }

        [Fact]
        public async Task Get_should_return_category()
        {
            // Arrange
            int id = 1;
            Category category = new Category() { Id = id };
            // Act
            _categoryRepository.Setup(x => x.Get(It.IsAny<int>())).ReturnsAsync(category).Verifiable();
            var result = await _service.Get(id);
            // Assert
            Assert.NotNull(result);
            Assert.IsType<Category>(result);
            Assert.Equal(id, result.Id);
            _categoryRepository.VerifyAll();
        }
    }
}
