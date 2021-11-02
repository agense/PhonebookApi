using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using PhonebookBusiness.Interfaces;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PhonebookApi.Authorization
{
    public class ContactOwnerAuthorizationHandler : AuthorizationHandler<ContactOwnerRequirement>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContactRepository _contactRepository;

        public ContactOwnerAuthorizationHandler(IHttpContextAccessor httpContextAccessor, IContactRepository contactRepository)
        {
            _httpContextAccessor = httpContextAccessor;
            _contactRepository = contactRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ContactOwnerRequirement requirement)
        {

            if (! Int32.TryParse(_httpContextAccessor.HttpContext.User.FindFirstValue("Id"), out int authUserId)) 
            {
                context.Fail();
            }

            var resource = _httpContextAccessor.HttpContext.GetRouteValue("key");

            if (resource == null)
            {
                context.Fail();
            }
            if (!Int32.TryParse(resource.ToString(), out int resourceId)) {
                context.Fail();
            }

            if (await _contactRepository.ExistsInUsersOwnedContacts(resourceId, authUserId))
            {
                context.Succeed(requirement);
            }
            else {
                context.Fail();
            }
        }
    }
}