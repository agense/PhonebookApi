using PhonebookBusiness.Models;
using PhonebookData.Interfaces;
using System.Collections.Generic;

namespace PhonebookData.Mappers
{
    public class ContactDataToBusinessModelMapper: IContactDataToBusinessModelMapper
    {
        private readonly IContactListItemDataToBusinessModelMapper _contactListItemMapper;
        
        public ContactDataToBusinessModelMapper(IContactListItemDataToBusinessModelMapper contactListItemMapper)
        {
            _contactListItemMapper = contactListItemMapper;
        }

        public Contact Map(Models.Contact contact)
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

        public Contact MapWithRelations(Models.Contact contact)
        {
            var businessModel = Map(contact);
            businessModel.UserContactListItems = new List<UserContactListItem>();

            if (contact.UserContactList!= null && contact.UserContactList.Count > 0)
            {
                foreach (var contactListItem in contact.UserContactList)
                {
                    businessModel.UserContactListItems.Add(_contactListItemMapper.Map(contactListItem));
                }
            }
            return businessModel;
        }

       public IEnumerable<Contact> MapList(IEnumerable<Models.Contact> contactList)
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
