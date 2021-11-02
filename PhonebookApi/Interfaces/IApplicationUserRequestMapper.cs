using PhonebookApi.Dto.Requests;
using PhonebookBusiness.Models;

namespace PhonebookApi.Interfaces
{

    public interface IApplicationUserRequestMapper
    {
        public ApplicationUser Map(RegistrationRequest request);
    }

}


