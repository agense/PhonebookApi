using Microsoft.AspNetCore.Http;
using PhonebookBusiness.Interfaces;
using System;
using System.Security.Claims;


namespace PhonebookApi.Services
{
    public class AuthenticatedUserService : IAuthenticatedUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int AuthenticatedUserId
        {
            get { return GetAuthenticatedUserId(); }
        }

        private int GetAuthenticatedUserId() {
            if (Int32.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"), out int result))
            {
                return result;
            }
            else {
                throw new Exception("Failed to retrieve authenticated user");
            }    
        } 

    }
}
