using PhonebookApi.Dto.Responses;
using PhonebookBusiness.Models;

namespace PhonebookApi.Interfaces
{

    public interface IAuthenticationResponseMapper
    {
        public AuthenticationResponse Map(ApplicationUser model, string token);

    }

}