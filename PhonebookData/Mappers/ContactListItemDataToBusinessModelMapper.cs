using PhonebookBusiness.Models;
using PhonebookData.Interfaces;
using PhonebookData.Models;

namespace PhonebookData.Mappers
{
    public class ContactListItemDataToBusinessModelMapper : IContactListItemDataToBusinessModelMapper
    {
        public UserContactListItem Map(UserContactList contactListItem) 
        {

            return new UserContactListItem()
            {
                Id = contactListItem.Id,
                ContactId = contactListItem.ContactId,
                UserId = contactListItem.UserId,
                OwnerId = contactListItem.OwnerId,
                Contact = new PhonebookBusiness.Models.Contact()
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
