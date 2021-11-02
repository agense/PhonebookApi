using PhonebookApi.Dto.Requests;
using PhonebookBusiness.Models;

namespace PhonebookApi.Interfaces
{

    public interface IContactRequestMapper
    {
        public Contact Map(ContactRequest request, int ownerId);
    }

}
