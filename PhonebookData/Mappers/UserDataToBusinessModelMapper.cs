using PhonebookData.Interfaces;
using PhonebookData.Models;
using System.Collections.Generic;
using System.Linq;

namespace PhonebookData.Mappers
{
    public class UserDataToBusinessModelMapper : IUserDataToBusinessModelMapper
    {
        private readonly IContactListItemDataToBusinessModelMapper _contactListItemMapper;

        public UserDataToBusinessModelMapper(IContactListItemDataToBusinessModelMapper contactListItemMapper)
        {
            _contactListItemMapper = contactListItemMapper;
        }

        public PhonebookBusiness.Models.ApplicationUser Map(ApplicationUser user)
        {
            return new PhonebookBusiness.Models.ApplicationUser()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
               
            };
        }

        public PhonebookBusiness.Models.ApplicationUser MapWithRelations(ApplicationUser user)
        {
            PhonebookBusiness.Models.ApplicationUser userModel = Map(user);
            userModel.UserContactListItems = new List<PhonebookBusiness.Models.UserContactListItem>();

            if (user.UserContactList != null && user.UserContactList.Count() > 0)
            {
                foreach (var contactListItem in user.UserContactList)
                {
                    userModel.UserContactListItems.Add(_contactListItemMapper.Map(contactListItem));
                }
            }
            return userModel;
        }

        public IEnumerable<PhonebookBusiness.Models.ApplicationUser> MapList(IEnumerable<ApplicationUser> userList)
        {
            List<PhonebookBusiness.Models.ApplicationUser> users = new List<PhonebookBusiness.Models.ApplicationUser>();
            foreach (var user in userList)
            {
                users.Add(Map(user));
            }
            return users;
        }
    }
}
