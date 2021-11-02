using FluentAssertions;
using PhonebookApi.Dto.Requests;
using PhonebookApi.Dto.Responses;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookTests.Integration
{
    public class AuthControllerTests : IntegrationTests
    {
        [Fact]
        public async Task Register_WithCorrectCredentials_ReturnsOk()
        {
            var request = new RegistrationRequest()
            {
                FirstName = Helpers.GetAlpha(),
                LastName = Helpers.GetAlpha(),
                Email = $"{Helpers.GetAlpha()}@test.com",
                Password = "Test-00",
                PasswordConfirmation = "Test-00",
            };
            var response = await _testClient.PostAsJsonAsync("api/Auth/Register", request);
            var data = await response.Content.ReadAsAsync<AuthenticationResponse>();

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            data.Should().NotBeNull();
            Assert.Equal(request.Email, data.User.Email);
        }
    }
}
