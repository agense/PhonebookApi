using PhonebookApi.Dto.Responses;
using PhonebookBusiness.Models;
using System.Collections.Generic;

namespace PhonebookApi.Interfaces
{

    public interface IContactResponseMapper
    {

        public ContactResponse Map(Contact model);

        public IEnumerable<ContactResponse> MapList(IEnumerable<Contact> contactList);


    }

}
