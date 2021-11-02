using PhonebookBusiness.Models;
using PhonebookData.Models;

namespace PhonebookData.Interfaces
{
    public interface IContactListItemDataToBusinessModelMapper
    {
        public UserContactListItem Map(UserContactList contactListItem);
    }
}
