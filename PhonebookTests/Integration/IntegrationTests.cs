using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PhonebookApi;
using PhonebookApi.Dto.Requests;
using PhonebookApi.Dto.Responses;
using PhonebookData;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace PhonebookTests.Integration
{
    public class IntegrationTests
    {
        protected readonly HttpClient _testClient;
        protected readonly AppDbContext _testDbContext;

        protected IntegrationTests()
        {
            WebApplicationFactory<Startup> webhost = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => {
                    builder.ConfigureTestServices(services =>
                    {
                        var dbContextDescriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<AppDbContext>)
                        );
                        services.Remove(dbContextDescriptor);
                        services.AddDbContext<AppDbContext>(options =>
                                options.UseInMemoryDatabase("phonebook_test_db")
                        );
                    });
                });

            _testDbContext = webhost.Services.CreateScope().ServiceProvider.GetService<AppDbContext>();
            _testClient = webhost.CreateClient();
        }

        protected async Task<ApplicationUserInDetailResponse> Authenticate() {
            var response = await GetJwt();
            var data =  await response.Content.ReadAsAsync<AuthenticationResponse>();
            _testClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", data.Token);
            return data.User;
        }

        private async Task<HttpResponseMessage> GetJwt() {
            var request = new RegistrationRequest()
            {
                FirstName = Helpers.GetAlpha(),
                LastName = Helpers.GetAlpha(),
                Email = $"{Helpers.GetAlpha()}@test.com",
                Password = "Test-00",
                PasswordConfirmation = "Test-00",
            };
            var response = await _testClient.PostAsJsonAsync("api/Auth/Register", request);
            return response;
        }

    }
}
