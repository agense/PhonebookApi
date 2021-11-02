using FluentAssertions;
using PhonebookApi.Dto.Responses;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookTests.Integration
{
    public class UsersControllerTests : IntegrationTests
    {
        private readonly Random rand = new();


        [Fact]
        public async Task GetOne_WithoutAuthentication_ReturnsUnauthenticated()
        {
            var response = await _testClient.GetAsync($"api/Users/{rand.Next()}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        //As there is no user, middleware requiring the user to be account owner does not allow the request through
        [Fact]
        public async Task GetOne_GetNotSelfAccountData_ReturnsForbidden()
        {
            var user = await Authenticate();
            var randomId = 1;
            do
            {
                randomId = rand.Next();
            } while (randomId == user.Id);
            var response = await _testClient.GetAsync($"api/Users/{randomId}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetOne_GetSelfAccountData_ReturnsOk()
        {
            var user = await Authenticate();

            var response = await _testClient.GetAsync($"api/Users/{user.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadAsAsync<ApplicationUserResponse>();
            Assert.Equal(user.Id, data.Id);
        }

    }
}
