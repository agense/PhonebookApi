using PhonebookApi.Dto.Responses;
using PhonebookBusiness.Models;
using System.Collections.Generic;

namespace PhonebookApi.Interfaces
{

    public interface IApplicationUserResponseMapper
    {
        public ApplicationUserResponse Map(ApplicationUser model);

        public IEnumerable<ApplicationUserResponse> MapList(IEnumerable<ApplicationUser> userList);
    }

}