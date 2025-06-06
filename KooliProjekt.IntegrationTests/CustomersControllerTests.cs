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
    public class CustomersControllerTests : TestBase
    {
        private readonly HttpClient _client;
        private readonly Mock<ICustomerService> _customerServiceMock;

        public CustomersControllerTests()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false
            };
            _client = Factory.CreateClient(options);

            _customerServiceMock = new Mock<ICustomerService>();
        }

        [Fact]
        public async Task Details_should_return_notfound_when_id_is_null()
        {
            var response = await _client.GetAsync("/Customers/Details/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Details_should_return_notfound_when_customer_does_not_exist()
        {
            int nonExistingId = 999;
            _customerServiceMock.Setup(s => s.Get(nonExistingId)).ReturnsAsync((Customer)null);

            var response = await _client.GetAsync($"/Customers/Details/{nonExistingId}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Create_should_redirect_on_success()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Name", "New Customer" },
                { "Email", "customer@example.com" },
                { "Phone", "5551234" },
                { "Address", "Tallinn" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            var response = await _client.PostAsync("/Customers/Create", content);
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }

        [Fact]
        public async Task Create_should_return_view_when_model_invalid()
        {
            var formValues = new Dictionary<string, string>
            {
                { "Name", "" }, // nimi tühi
                { "Email", "invalid" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            var response = await _client.PostAsync("/Customers/Create", content);
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
                { "Name", "Changed Name" },
                { "Email", "test@x.ee" },
                { "Phone", "12345678" },
                { "Address", "New Street" }
            };
            using var content = new FormUrlEncodedContent(formValues);

            var response = await _client.PostAsync("/Customers/Edit/999", content);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_id_is_null()
        {
            var response = await _client.GetAsync("/Customers/Delete/");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task Delete_should_return_notfound_when_customer_does_not_exist()
        {
            int id = 999;
            _customerServiceMock.Setup(s => s.Get(id)).ReturnsAsync((Customer)null);

            var response = await _client.GetAsync($"/Customers/Delete/{id}");
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteConfirmed_should_redirect()
        {
            int id = 100;
            var customer = new Customer { Id = id, Name = "ToDelete" };
            _customerServiceMock.Setup(s => s.Get(id)).ReturnsAsync(customer);

            var response = await _client.PostAsync($"/Customers/Delete/{id}", null);
            Assert.True(
                response.StatusCode == HttpStatusCode.Redirect ||
                response.StatusCode == HttpStatusCode.MovedPermanently);
        }
    }
}
