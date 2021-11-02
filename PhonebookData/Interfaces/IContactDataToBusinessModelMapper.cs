using PhonebookData.Models;
using System.Collections.Generic;

namespace PhonebookData.Interfaces
{
    public interface IContactDataToBusinessModelMapper
    {
        public PhonebookBusiness.Models.Contact Map(Contact contact);

        public PhonebookBusiness.Models.Contact MapWithRelations(Contact contact);

        public IEnumerable<PhonebookBusiness.Models.Contact> MapList(IEnumerable<Contact> contacts);
    }
}
