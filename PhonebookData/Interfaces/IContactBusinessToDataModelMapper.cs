using PhonebookData.Models;
using System.Collections.Generic;

namespace PhonebookData.Interfaces
{
    public interface IContactBusinessToDataModelMapper
    {
        public Contact Map(PhonebookBusiness.Models.Contact contact);

        public Contact MapWithRelations(PhonebookBusiness.Models.Contact contact);

        public IEnumerable<Contact> MapList(IEnumerable<PhonebookBusiness.Models.Contact> contacts);
    }
}
