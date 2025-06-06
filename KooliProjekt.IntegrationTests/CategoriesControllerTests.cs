using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class CategoriesControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly Mock<ICategoryItemService> _categoryServiceMock;

        public CategoriesControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);

            // Võid teenust mockida kui kasutad DI konfiguratsioonis (vajadusel tee teenuse override testide ajal)
            _categoryServiceMock = new Mock<ICategoryItemService>();
        }
        
        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            // Act
            var response = await _client.GetAsync("/Categories/Details/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_category_does_not_exist()
        {
            // Arrange
            int nonExistingId = 999;
            _categoryServiceMock.Setup(s => s.Get(nonExistingId)).ReturnsAsync((Category)null);

            // Act
            var response = await _client.GetAsync($"/Categories/Details/{nonExistingId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }


        [Fact]
        public async Task Create_should_redirect_on_success()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Name", "New Category" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            // Act
            var response = await _client.PostAsync("/Categories/Create", content);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }

        [Fact]
        public async Task Create_should_return_view_when_model_invalid()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Name", "" }  // Tühja nimega ei tohiks salvestada
            };
            using var content = new FormUrlEncodedContent(formValues);

            // Act
            var response = await _client.PostAsync("/Categories/Create", content);

            // Assert
            Assert.True(response.StatusCode == HttpStatusCode.OK);
            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Create", responseString); // peaks tagasi vormile näitama
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_mismatch()
        {
            // Arrange
            var formValues = new Dictionary<string, string>
            {
                { "Id", "1" },
                { "Name", "Changed" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            // Act
            var response = await _client.PostAsync("/Categories/Edit/999", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
        

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            // Act
            var response = await _client.GetAsync("/Categories/Delete/");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_category_does_not_exist()
        {
            // Arrange
            int id = 999;
            _categoryServiceMock.Setup(s => s.Get(id)).ReturnsAsync((Category)null);

            // Act
            var response = await _client.GetAsync($"/Categories/Delete/{id}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect()
        {
            // Arrange
            int id = 100;
            var category = new Category { Id = id, Name = "ToDelete" };
            _categoryServiceMock.Setup(s => s.Get(id)).ReturnsAsync(category);

            // Act
            var response = await _client.PostAsync($"/Categories/Delete/{id}", null);

            // Assert
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }
    }
}
