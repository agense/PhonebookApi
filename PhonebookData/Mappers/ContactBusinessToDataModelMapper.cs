using PhonebookData.Interfaces;
using PhonebookData.Models;
using System.Collections.Generic;

namespace PhonebookData.Mappers
{
    public class ContactBusinessToDataModelMapper : IContactBusinessToDataModelMapper
    {
        private readonly IPhonebookBusinessToDataContactListItemMapper _contactListMapper;

        public ContactBusinessToDataModelMapper(IPhonebookBusinessToDataContactListItemMapper contactListMapper)
        {
            _contactListMapper = contactListMapper;
        }

        public Contact Map(PhonebookBusiness.Models.Contact contact)
        {
            return new Contact()
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                CountryCode = contact.CountryCode,
                Phone = contact.Phone,
                OwnerId = contact.OwnerId
            };
        }

        public Contact MapWithRelations(PhonebookBusiness.Models.Contact contact)
        {
            var dataModel = Map(contact);

            if (contact.UserContactListItems != null && contact.UserContactListItems.Count > 0)
            {
                foreach (var contactListItem in contact.UserContactListItems)
                {
                    dataModel.UserContactList.Add(_contactListMapper.Map(contactListItem));
                }
            }
            return dataModel;
        }

        public IEnumerable<Contact> MapList(IEnumerable<PhonebookBusiness.Models.Contact> contactList)
        {
            List<Contact> contacts = new List<Contact>();
            foreach (var contact in contactList)
            {
                contacts.Add(Map(contact));
            }
            return contacts;
        }
    }
}
