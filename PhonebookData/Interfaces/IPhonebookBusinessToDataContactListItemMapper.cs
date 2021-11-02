using PhonebookBusiness.Models;
using PhonebookData.Models;

namespace PhonebookData.Interfaces
{
    public interface IPhonebookBusinessToDataContactListItemMapper
    {
        public UserContactList Map(UserContactListItem contactListItem);
    }
}
