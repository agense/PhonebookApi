using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Models;

namespace PhonebookApi.Mappers
{

    public class AuthenticationResponseMapper : IAuthenticationResponseMapper
    {
        private readonly IApplicationUserInDetailResponseMapper _userInDetailMapper;

        public AuthenticationResponseMapper(IApplicationUserInDetailResponseMapper userInDetailMapper)
        {
            _userInDetailMapper = userInDetailMapper;
        }

        public AuthenticationResponse Map(ApplicationUser model, string token)
        {
            return new AuthenticationResponse()
            {
                User = _userInDetailMapper.Map(model),
                Token = token
            };
        }
    }

}
