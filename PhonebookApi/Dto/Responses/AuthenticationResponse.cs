using PhonebookBusiness.Models;

namespace PhonebookApi.Dto.Responses
{
    public class AuthenticationResponse
    {
        public ApplicationUserInDetailResponse User { get; set; }

        public string Token { get; set; }
    }
}