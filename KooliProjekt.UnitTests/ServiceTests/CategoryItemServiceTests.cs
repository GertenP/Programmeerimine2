using KooliProjekt.Data;
using KooliProjekt.Search;
using KooliProjekt.Services;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CategoryItemServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Get_by_id_should_return_category()
        {
            // Arrange
            var category = new Category { Name = "TestCategory" };
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();

            var service = new CategoryItemService(DbContext);

            // Act
            var result = await service.Get(category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestCategory", result.Name);
        }

        [Fact]
        public async Task Get_by_nullable_id_should_return_category()
        {
            // Arrange
            var category = new Category { Name = "TestCategory" };
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();

            var service = new CategoryItemService(DbContext);

            // Act
            var result = await service.Get((int?)category.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("TestCategory", result.Name);
        }

        [Fact]
        public async Task Get_should_return_null_if_not_found()
        {
            // Arrange
            var service = new CategoryItemService(DbContext);

            // Act
            var result = await service.Get(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Includes_should_return_true_if_category_exists()
        {
            // Arrange
            var category = new Category { Name = "TestCategory" };
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();

            var service = new CategoryItemService(DbContext);

            // Act
            var exists = await service.Includes(category.Id);

            // Assert
            Assert.True(exists);
        }

        [Fact]
        public async Task Includes_should_return_false_if_category_does_not_exist()
        {
            // Arrange
            var service = new CategoryItemService(DbContext);

            // Act
            var exists = await service.Includes(999);

            // Assert
            Assert.False(exists);
        }

        [Fact]
        public async Task Delete_should_remove_category_if_exists()
        {
            // Arrange
            var category = new Category { Name = "ToDelete" };
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();

            var service = new CategoryItemService(DbContext);

            // Act
            await service.Delete(category.Id);

            // Assert
            Assert.Empty(DbContext.Categories);
        }

        [Fact]
        public async Task Delete_should_do_nothing_if_category_not_found()
        {
            // Arrange
            var service = new CategoryItemService(DbContext);

            // Act
            await service.Delete(999); // does not exist

            // Assert
            Assert.Empty(DbContext.Categories);
        }

        [Fact]
        public async Task Save_should_add_new_category_if_id_is_zero()
        {
            // Arrange
            var category = new Category { Name = "NewCategory" };
            var service = new CategoryItemService(DbContext);

            // Act
            await service.Save(category);

            // Assert
            Assert.Equal(1, DbContext.Categories.Count());
            Assert.Equal("NewCategory", DbContext.Categories.First().Name);
        }

        [Fact]
        public async Task Save_should_update_existing_category()
        {
            // Arrange
            var category = new Category { Name = "OldName" };
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();

            category.Name = "UpdatedName";
            var service = new CategoryItemService(DbContext);

            // Act
            await service.Save(category);

            // Assert
            Assert.Equal(1, DbContext.Categories.Count());
            Assert.Equal("UpdatedName", DbContext.Categories.First().Name);
        }

        [Fact]
        public async Task List_should_return_all_categories_without_filter()
        {
            // Arrange
            for (int i = 1; i <= 10; i++)
            {
                DbContext.Categories.Add(new Category { Name = "Cat" + i });
            }
            DbContext.SaveChanges();

            var service = new CategoryItemService(DbContext);

            // Act
            var result = await service.List(1, 5);

            // Assert
            Assert.Equal(5, result.Results.Count);
            Assert.Equal(10, result.RowCount);
        }

        [Fact]
        public async Task List_should_filter_by_keyword()
        {
            // Arrange
            DbContext.Categories.Add(new Category { Name = "Apple" });
            DbContext.Categories.Add(new Category { Name = "Banana" });
            DbContext.SaveChanges();

            var service = new CategoryItemService(DbContext);
            var search = new CategorySearch { Keyword = "App" };

            // Act
            var result = await service.List(1, 10, search);

            // Assert
            Assert.Single(result.Results);
            Assert.Equal("Apple", result.Results.First().Name);
        }
    }
}
