using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Models;
using System.Collections.Generic;

namespace PhonebookApi.Mappers
{

    public class ApplicationUserResponseMapper : IApplicationUserResponseMapper
    {

        public ApplicationUserResponse Map(ApplicationUser model)
        {
            return new ApplicationUserResponse()
            {
                Id = model.Id,
                Name = $"{model.FirstName} {model.LastName}"
            };
        }

        public IEnumerable<ApplicationUserResponse> MapList(IEnumerable<ApplicationUser> userList)
        {
            List<ApplicationUserResponse> users = new List<ApplicationUserResponse>();
            foreach (var user in userList)
            {
                users.Add(Map(user));
            }
            return users;
        }
    }

}
