using PhonebookData.Models;
using System.Collections.Generic;

namespace PhonebookData.Interfaces
{
    public interface IUserDataToBusinessModelMapper
    {

        public PhonebookBusiness.Models.ApplicationUser Map(ApplicationUser user);

        public PhonebookBusiness.Models.ApplicationUser MapWithRelations(ApplicationUser user);

        public IEnumerable<PhonebookBusiness.Models.ApplicationUser> MapList(IEnumerable<ApplicationUser> userList);
    }
}
