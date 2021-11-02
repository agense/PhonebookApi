using PhonebookApi.Dto.Requests;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Models;

namespace PhonebookApi.Mappers
{

    public class ApplicationUserRequestMapper : IApplicationUserRequestMapper
    {

        public ApplicationUser Map(RegistrationRequest request)
        {
            return new ApplicationUser()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password
            };
        }
    }

}
