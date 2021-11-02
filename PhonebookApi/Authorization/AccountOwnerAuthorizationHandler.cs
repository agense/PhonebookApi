using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PhonebookBusiness.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhonebookApi.Authorization
{
    public class AccountOwnerAuthorizationHandler : AuthorizationHandler<AccountOwnerRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IApplicationUserRepository _userRepository;

        public AccountOwnerAuthorizationHandler(
            IHttpContextAccessor httpContextAccessor, IApplicationUserRepository userRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, AccountOwnerRequirement requirement)
        {

            if (!Int32.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"), out int authUserId))
            {
                context.Fail();
            }

            var resource = _httpContextAccessor.HttpContext.GetRouteValue("key");

            if (resource == null)
            {
                context.Fail();
            }
            if (!Int32.TryParse(resource.ToString(), out int resourceId))
            {
                context.Fail();
            }

            if (await _userRepository.IsAccountOwner(resourceId, authUserId)) 
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}