using PhonebookApi.Dto.Responses;
using PhonebookApi.Interfaces;
using PhonebookBusiness.Models;

namespace PhonebookApi.Mappers
{

    public class ApplicationUserInDetailResponseMapper : IApplicationUserInDetailResponseMapper
    {

        public ApplicationUserInDetailResponse Map(ApplicationUser model)
        {
            return new ApplicationUserInDetailResponse
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
            };
        }
    }

}
