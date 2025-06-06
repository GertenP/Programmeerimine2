using System;
using KooliProjekt.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace KooliProjekt.IntegrationTests.Helpers
{
    public abstract class TestBase : IDisposable
    {
        public WebApplicationFactory<FakeStartup> Factory { get; }

        public TestBase()
        {
            Factory = new TestApplicationFactory<FakeStartup>();

            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureCreated();
        }

        public void Dispose()
        {
            using var scope = Factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            dbContext.Database.EnsureDeleted();
        }

        // Lisa vajadusel teisi abimeetodeid
    }
}