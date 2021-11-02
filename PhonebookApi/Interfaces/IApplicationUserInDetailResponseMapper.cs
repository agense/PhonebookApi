using PhonebookApi.Dto.Responses;
using PhonebookBusiness.Models;

namespace PhonebookApi.Interfaces
{

    public interface IApplicationUserInDetailResponseMapper
    {
        public ApplicationUserInDetailResponse Map(ApplicationUser model);

    }

}
