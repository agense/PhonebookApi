using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PhonebookApi.Controllers;
using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Interfaces;
using PhonebookBusiness.Models;
using System;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookTests.Unit
{
    public class UsersControllerTests
    {
        private readonly Mock<ILogger<UsersController>> loggerStub = new();
        private readonly Mock<IApplicationUserResponseMapper> userResponseMapperStub = new();
        private readonly Mock<IApplicationUserInDetailResponseMapper> userInDetailResponseMapperStub = new();
        private readonly Mock<IMessageResponseMapper> messageResponseMapperStub = new();
        private readonly Mock<IApplicationUserRepository> repositoryStub = new();
        private readonly Random rand = new();

        private ApplicationUser CreateRandomUser() {
            return new ApplicationUser() {
               Id = 1,
               FirstName = "TestFirst",
               LastName = "TestLast",
               Email = "test@test.com",
            };
        }

        [Fact]
        public async Task GetOne_WithNonExistingItem_ReturnsNotFound()
        {
            //Arrange
            repositoryStub.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync(false);

            var controller = new UsersController(
                loggerStub.Object, 
                repositoryStub.Object, 
                userResponseMapperStub.Object,
                userInDetailResponseMapperStub.Object,
                messageResponseMapperStub.Object
               );

            //Act
            var result = await controller.GetOne(rand.Next(10));

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);

        }

        [Fact]
        public async Task GetOne_WithExistingItem_ReturnsOk()
        {
            //Arrange
            var existingUser = CreateRandomUser();

            repositoryStub.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync(true);
            repositoryStub.Setup(repo => repo.GetOneById(It.IsAny<int>())).ReturnsAsync(existingUser);

            var controller = new UsersController(
                loggerStub.Object,
                repositoryStub.Object,
                userResponseMapperStub.Object,
                userInDetailResponseMapperStub.Object,
                messageResponseMapperStub.Object
               );

            //Act
            var result= await controller.GetOne(1);

            // Assert
            Assert.IsType<ActionResult<ApplicationUserResponse>>(result);
        }


    }
}
