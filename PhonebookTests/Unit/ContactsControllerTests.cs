using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using PhonebookApi.Controllers;
using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Interfaces;
using PhonebookBusiness.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace PhonebookTests.Unit
{
    public class ContactsControllerTests
    {
         private readonly Mock<ILogger<ContactsController>> loggerStub = new();
         private readonly Mock<IAuthenticatedUser> authUserStub = new();
         private readonly Mock<IContactRepository> repositoryStub = new();
         private readonly Mock<IContactResponseMapper> contactResponseMapperStub = new();
         private readonly Mock<IContactInDetailResponseMapper> contactInDetailResponseMapperStub = new();
         private readonly Mock<IContactRequestMapper> contactRequestMapperStub = new();
         private readonly Mock<IApplicationUserRepository> userRepositoryStub = new();
         private readonly Mock<IApplicationUserResponseMapper> userResponseMapperStub = new();
         private readonly Mock<IMessageResponseMapper> messageResponseMapperStub = new();

        private readonly Random rand = new();

        [Fact]
        public async Task GetOne_WithNonExistingItem_ReturnsNotFound()
        {
            //Arrange
            authUserStub.Setup(u => u.AuthenticatedUserId).Returns(1);
            repositoryStub.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync(false);

            var controller = new ContactsController(
                loggerStub.Object,
                authUserStub.Object,
                repositoryStub.Object,
                contactResponseMapperStub.Object,
                contactInDetailResponseMapperStub.Object,
                contactRequestMapperStub.Object,
                userRepositoryStub.Object,
                userResponseMapperStub.Object,
                messageResponseMapperStub.Object
               );

            //Act
            var result = await controller.GetOne(rand.Next(10));

            //Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetOne_WithExistingItem_ReturnsExistingItem()
        {
            //Arrange
            var expected = CreateExampleContact();

            authUserStub.Setup(u => u.AuthenticatedUserId).Returns(1);
            repositoryStub.Setup(repo => repo.Exists(It.IsAny<int>())).ReturnsAsync(true);
            repositoryStub.Setup(repo => repo.GetOneById(It.IsAny<int>())).ReturnsAsync(expected);

            var controller = new ContactsController(
                loggerStub.Object,
                authUserStub.Object,
                repositoryStub.Object,
                contactResponseMapperStub.Object,
                contactInDetailResponseMapperStub.Object,
                contactRequestMapperStub.Object,
                userRepositoryStub.Object,
                userResponseMapperStub.Object,
                messageResponseMapperStub.Object
               );

            //Act
            var result = await controller.GetOne(rand.Next(10));

            //Assert
            Assert.IsType<ActionResult<ContactInDetailResponse>>(result);
        }

        [Fact]
        public async Task Get_WithExistingItems_ReturnsExistingItems()
        {
            //Arrange
            var expected = new[] { CreateExampleContact(), CreateExampleContact(), CreateExampleContact() };                

            authUserStub.Setup(u => u.AuthenticatedUserId).Returns(1);
            repositoryStub.Setup(repo => repo.GetUsersContacts(1)).ReturnsAsync(expected);

            var controller = new ContactsController(
                loggerStub.Object,
                authUserStub.Object,
                repositoryStub.Object,
                contactResponseMapperStub.Object,
                contactInDetailResponseMapperStub.Object,
                contactRequestMapperStub.Object,
                userRepositoryStub.Object,
                userResponseMapperStub.Object,
                messageResponseMapperStub.Object
               );

            //Act
            var result = await controller.Get();

            //Assert
            Assert.IsType<ActionResult<IEnumerable<ContactResponse>>>(result);
        }

        [Fact]
        public async Task Create_WithValidRequest_ReturnsCreatedItem()
        {
            var model = CreateExampleContact();
            //Arrange
            var contactRequest = new PhonebookApi.Dto.Requests.ContactRequest() { 
                FirstName = model.FirstName,
                LastName = model.LastName,
                CountryCode = model.CountryCode,
                Phone = model.Phone
            };
            
            authUserStub.Setup(u => u.AuthenticatedUserId).Returns(1);
            repositoryStub.Setup(repo => repo.Create(model)).ReturnsAsync(model);

            var controller = new ContactsController(
                loggerStub.Object,
                authUserStub.Object,
                repositoryStub.Object,
                contactResponseMapperStub.Object,
                contactInDetailResponseMapperStub.Object,
                contactRequestMapperStub.Object,
                userRepositoryStub.Object,
                userResponseMapperStub.Object,
                messageResponseMapperStub.Object
               );

            //Act
            var result = await controller.Create(contactRequest);

            //Assert
            Assert.IsType<ActionResult<ContactInDetailResponse>>(result);
        }

        private Contact CreateExampleContact() 
        {
            return new Contact()
            {
                FirstName = Helpers.GetAlpha(),
                LastName = Helpers.GetAlpha(),
                CountryCode = Helpers.GetNumeric(3),
                Phone = Helpers.GetNumeric(),
                OwnerId = 1,
            };
        }
    }
}
