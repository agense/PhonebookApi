using PhonebookData.Models;
using System.Collections.Generic;

namespace PhonebookData.Interfaces
{
    public interface IUserBusinessToDataModelMapper
    {
        public ApplicationUser Map(PhonebookBusiness.Models.ApplicationUser user);

        public ApplicationUser MapWithRelations(PhonebookBusiness.Models.ApplicationUser user);

        public IEnumerable<ApplicationUser> MapList(IEnumerable<PhonebookBusiness.Models.ApplicationUser> userList);
    }
}
