using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace CESIZen.Test
{
    public class UrlTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UrlTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Theory]
        [InlineData("/")]
        [InlineData("/Home/Index")]
        [InlineData("/Account/Login")]
        public async Task Urls_ReturnSuccessStatusCode(string url)
        {
            // Act
            var response = await _client.GetAsync(url);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
