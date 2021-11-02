using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Interfaces;
using PhonebookBusiness.Models;
using System.Collections.Generic;

namespace PhonebookApi.Mappers {
    
    public class ContactResponseMapper : IContactResponseMapper
    {
        private readonly IAuthenticatedUser _authUserService;

        public ContactResponseMapper(IAuthenticatedUser authUserService)
        {
            _authUserService = authUserService;
        }

        public ContactResponse Map(Contact model)
        {
            var response = new ContactResponse()
            {
                Id = model.Id,
                Name = $"{model.FirstName} {model.LastName}",
                Phone = $"{model.CountryCode}{model.Phone}",
            };

            if (model.OwnerId != 0 && model.OwnerId != null && _authUserService.AuthenticatedUserId != 0)
            {
                response.IsOwnedContact = model.OwnerId == _authUserService.AuthenticatedUserId ? true : false;
            }
            return response;
        }

        public IEnumerable<ContactResponse> MapList(IEnumerable<Contact> contactList)
        {
            List<ContactResponse> contacts = new List<ContactResponse>();
            foreach (var contact in contactList)
            {
                contacts.Add(Map(contact));
            }
            return contacts;
        }
    }

}
