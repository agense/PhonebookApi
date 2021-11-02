using PhonebookApi.Dto.Responses;
using System.Collections.Generic;

namespace PhonebookApi.Interfaces
{

    public interface IContactInDetailResponseMapper
    {
        public ContactInDetailResponse Map(PhonebookBusiness.Models.Contact model);

        public IEnumerable<ContactInDetailResponse> MapList(IEnumerable<PhonebookBusiness.Models.Contact> contactList);

    }

}