using PhonebookBusiness.Models;
using PhonebookData.Interfaces;
using PhonebookData.Models;

namespace PhonebookData.Mappers
{
    public class ContactListItemBusinessToDataMapper : IPhonebookBusinessToDataContactListItemMapper
    {

        public UserContactList Map(UserContactListItem contactListItem) 
        {
            return new UserContactList()
            {
                Id = contactListItem.Id,
                ContactId = contactListItem.ContactId,
                UserId = contactListItem.UserId,
                OwnerId = contactListItem.OwnerId,
                Contact = new PhonebookData.Models.Contact()
                {
                    Id = contactListItem.Contact.Id,
                    FirstName = contactListItem.Contact.FirstName,
                    LastName = contactListItem.Contact.LastName,
                    CountryCode = contactListItem.Contact.CountryCode,
                    Phone = contactListItem.Contact.Phone
                }
            };
        }
    }     
}
