using FluentAssertions;
using PhonebookApi.Dto.Requests;
using PhonebookApi.Dto.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookTests.Integration
{
    public class ContactsControllerTests : IntegrationTests
    {
        private readonly Random rand = new();


        [Fact]
        public async Task Create_WithoutAuthentication_ReturnsUnauthenticated()
        {
            var response = await CreateContact(TestRequest());

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Create_WithCorrectData_ReturnsCreated()
        {
            await Authenticate();

            var request = TestRequest();
            var response = await CreateContact(request);

            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var data = await response.Content.ReadAsAsync<ContactResponse>();
            Assert.Equal(request.Phone, data.Phone);
            Assert.True(data.IsOwnedContact);
        }

        [Fact]
        public async Task Create_WithEmptyPhone_ReturnsBadRequest()
        {
            await Authenticate();

            var request = TestRequest();
            request.Phone = "";

            var response = await CreateContact(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Create_WithIncorrectPhone_ReturnsBadRequest()
        {
            await Authenticate();

            var request = TestRequest();
            request.Phone = Helpers.GetAlpha();

            var response = await CreateContact(request);

            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GetOne_WithoutAuthentication_ReturnsUnauthenticated()
        {
            var response = await _testClient.GetAsync($"api/Contacts/{rand.Next(9)}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        //As there is no contact, middleware requiring the contact to be in users contacts does not allow the request through
        [Fact]
        public async Task GetOne_GetNonExistant_ReturnsForbidden()
        {
            await Authenticate();

            var response = await _testClient.GetAsync($"api/Contacts/{rand.Next(9)}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task GetOne_GetExistant_ReturnsOk()
        {
            await Authenticate();

            var createdResponse = await CreateContact(TestRequest());
            var created = await createdResponse.Content.ReadAsAsync<ContactResponse>();

            var response = await _testClient.GetAsync($"api/Contacts/{created.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadAsAsync<ContactResponse>();
            Assert.Equal(created.Phone, data.Phone);
            Assert.Equal(created.Id, data.Id);
            Assert.Equal(created.Name, data.Name);
        }

        [Fact]
        public async Task GetAll_WithoutAuthentication_ReturnsUnauthenticated()
        {
            var response = await _testClient.GetAsync("api/Contacts");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetAll_WithAuthenticationAndNoItems_ReturnsOk()
        {
            await Authenticate();

            var response = await _testClient.GetAsync("api/Contacts");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadAsAsync<IEnumerable<ContactResponse>>();
            data.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAll_WithAuthenticationAndItems_ReturnsOk()
        {
            await Authenticate();

            var contactOneRespone = await CreateContact(TestRequest());
            var contactTwoRespone = await CreateContact(TestRequest());

            var response = await _testClient.GetAsync("api/Contacts");

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadAsAsync<IEnumerable<ContactResponse>>();
            Assert.Equal(2, data.Count());
        }

        [Fact]
        public async Task Update_WithoutAuthentication_ReturnsUnauthenticated()
        {
            var request = TestRequest();

            var response = await _testClient.PutAsJsonAsync($"api/Contacts/{rand.Next(9)}", request);

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Update_WithAuthenticationNonExistant_ReturnsForbidden()
        {
            await Authenticate();

            var request = TestRequest();

            var response = await _testClient.PutAsJsonAsync($"api/Contacts/{rand.Next(9)}", request);

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Update_WithAuthenticationExistant_ReturnsOk()
        {
            await Authenticate();

            var createdResponse = await CreateContact(TestRequest());
            var created = await createdResponse.Content.ReadAsAsync<ContactResponse>();

            var request = TestRequest();

            var response = await _testClient.PutAsJsonAsync($"api/Contacts/{created.Id}", request);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var data = await response.Content.ReadAsAsync<ContactResponse>();

            Assert.Equal(request.Phone, data.Phone);
        }

        [Fact]
        public async Task Delete_WithoutAuthentication_ReturnsUnauthenticated()
        {
            var response = await _testClient.DeleteAsync($"api/Contacts/{rand.Next(9)}");

            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task Delete_WithAuthenticationNonExistant_ReturnsForbidden()
        {
            await Authenticate();

            var response = await _testClient.DeleteAsync($"api/Contacts/{rand.Next(9)}");

            response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task Delete_WithAuthenticationExistant_ReturnsNoContent()
        {
            await Authenticate();

            var createdResponse = await CreateContact(TestRequest());
            var created = await createdResponse.Content.ReadAsAsync<ContactResponse>();

            var response = await _testClient.DeleteAsync($"api/Contacts/{created.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }


        private async Task<HttpResponseMessage> CreateContact(ContactRequest request) {
            
            var response = await _testClient.PostAsJsonAsync("api/Contacts", request);
            return response;
        }

        private ContactRequest TestRequest() {
            return new ContactRequest()
            {
                FirstName = Helpers.GetAlpha(),
                LastName = Helpers.GetAlpha(),
                CountryCode = Helpers.GetNumeric(4),
                Phone = Helpers.GetNumeric(),
            };
        }
    }
}
