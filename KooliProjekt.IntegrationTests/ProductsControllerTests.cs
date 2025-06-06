using KooliProjekt.Models;
using KooliProjekt.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using KooliProjekt.Data;
using KooliProjekt.IntegrationTests.Helpers;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    [Collection("Sequential")]
    public class ProductsControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly Mock<IProductService> _productServiceMock;

        public ProductsControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);

            _productServiceMock = new Mock<IProductService>();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            var response = await _client.GetAsync("/Products/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_product_does_not_exist()
        {
            int nonExistingId = 999;
            _productServiceMock.Setup(s => s.Get(nonExistingId)).ReturnsAsync((Product)null);

            var response = await _client.GetAsync($"/Products/Details/{nonExistingId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_redirect_on_success()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Name", "New Product" },
                { "Description", "A test product" },
                { "Price", "9.99" },
                { "CategoryId", "1" },
                { "Discount", "0" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            var response = await _client.PostAsync("/Products/Create", content);
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }

        [Fact]
        public async Task Create_should_return_view_when_model_invalid()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Name", "" }, // Required field is empty
                { "Price", "-1" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            var response = await _client.PostAsync("/Products/Create", content);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var responseString = await response.Content.ReadAsStringAsync();
            Assert.Contains("Create", responseString);
        }

        [Fact]
        public async Task Edit_should_return_notfound_when_id_mismatch()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Id", "1" },
                { "Name", "Updated Name" },
                { "Description", "Updated Description" },
                { "Price", "19.99" },
                { "CategoryId", "1" },
                { "Discount", "0" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            var response = await _client.PostAsync("/Products/Edit/999", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            var response = await _client.GetAsync("/Products/Delete/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_product_does_not_exist()
        {
            int id = 999;
            _productServiceMock.Setup(s => s.Get(id)).ReturnsAsync((Product)null);

            var response = await _client.GetAsync($"/Products/Delete/{id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect()
        {
            int id = 100;
            var product = new Product { Id = id, Name = "ToDelete" };
            _productServiceMock.Setup(s => s.Get(id)).ReturnsAsync(product);

            var response = await _client.PostAsync($"/Products/Delete/{id}", null);
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }
    }
}
