using PhonebookApi.Dto.Requests;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Models;

namespace PhonebookApi.Mappers
{

    public class ContactRequestMapper : IContactRequestMapper
    {

        public Contact Map(ContactRequest request, int ownerId)
        {
            return new Contact()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                CountryCode = request.CountryCode,
                Phone = request.Phone,
                OwnerId = ownerId
            };
        }
    }

}
