using KooliProjekt.Data;
using KooliProjekt.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.UnitTests.ServiceTests
{
    public class CategoryItemServiceTests : ServiceTestBase
    {
        [Fact]
        public async Task Delete_should_remove_given_list()
        {
            // Arrange

            var service = new CategoryItemService(DbContext);
            Category category = new Category() { Name = "Tere" };
            DbContext.Categories.Add(category);
            DbContext.SaveChanges();
            // Act
            await service.Delete(1);

            // Assert
            var count = DbContext.Categories.Count();
            Assert.Equal(0, count);
        }
    }
}
