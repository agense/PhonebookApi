using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Interfaces;
using PhonebookBusiness.Models;
using System.Collections.Generic;

namespace PhonebookApi.Mappers
{

    public class ContactInDetailResponseMapper : IContactInDetailResponseMapper
    {
        private readonly IAuthenticatedUser _authUserService;

        public ContactInDetailResponseMapper(IAuthenticatedUser authUserService)
        {
            _authUserService = authUserService;
        }

        public ContactInDetailResponse Map(Contact model)
        {
            var response = new ContactInDetailResponse()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CountryCode = model.CountryCode,
                Phone = model.Phone,
            };
            if (model.OwnerId != 0 && model.OwnerId != null && _authUserService.AuthenticatedUserId != 0)
            {
                response.IsOwnedContact = model.OwnerId == _authUserService.AuthenticatedUserId ? true : false;
            }
            return response;
        }

        public IEnumerable<ContactInDetailResponse> MapList(IEnumerable<Contact> contactList)
        {
            List<ContactInDetailResponse> contacts = new List<ContactInDetailResponse>();
            foreach (var contact in contactList)
            {
                contacts.Add(Map(contact));
            }
            return contacts;
        }
    }

}
